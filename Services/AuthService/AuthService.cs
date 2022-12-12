using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Contracts;
using Contracts.Repositories;
using Contracts.Security;
using Microsoft.IdentityModel.Tokens;
using Domain.Exceptions;
using Domain.Models;
using Domain.SecurityModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Services.AuthService;

public class AuthService : IAuthService
{
    private readonly IRepositoryManager _repository;
    private readonly IConfiguration _configuration;
    private readonly IMailSender _mailSender;
    private readonly ITokenService _tokenService;

    private string subject = "Подтверждение почты";
    private string hostLink = "http://localhost:5097/EmailConfirmation?token=";
    
    public AuthService(
        IRepositoryManager repository,
        IConfiguration configuration,
        IMailSender mailSender,
        ITokenService tokenService)
    {
        _repository = repository;
        _configuration = configuration;
        _mailSender = mailSender;
        _tokenService = tokenService;

        hostLink = _configuration.GetSection("LunchRoomDomainName").Value + "/EmailConfirmation?token=";
    }
    
    public async Task<string> Auth(string email)
    {
        var user = await _repository.User.GetUserByEmailAsync(email);
        if (user is null)
        {
            var newUser = new User(email);
            _repository.User.CreateUser(newUser);
            await _repository.SaveAsync();
            await SendConfirmationEmail(newUser.Email);
            return null;
        }
        else if (user.IsEmailChecked is false)
        {
            await SendConfirmationEmail(user.Email);
            return null;
        }
        else
        {
            var token = await _tokenService.Generate(user);
            return token;
        }
    }

    public async Task SendConfirmationEmail(string email)
    {
        var token = GenerateJwtTokenForEmailConfirmation(email);
        var link = hostLink + token;
        string body = string.Empty;
        using (StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + "/Html/email.html"))
        {
            body = reader.ReadToEnd();
        }

        body = body.Replace("{ConfirmationLink}", link);
        
        var emailValidation = await _repository.Security.GetEmailValidation(email);
        if (emailValidation is null)
        {
            
            var emailValidationNew = new EmailValidation(email, token);
            _repository.Security.CreateEmailValidation(emailValidationNew);
            await _repository.SaveAsync();
            
            await _mailSender.SendEmailAsync(email, subject, body);
        }
        else
        {
            _repository.Security.DeleteEmailValidation(emailValidation);
            var emailValidationNew = new EmailValidation(email, token);
            _repository.Security.CreateEmailValidation(emailValidationNew);
            await _repository.SaveAsync();
            
            await _mailSender.SendEmailAsync(email, subject, body);
        }
    }

    public async Task<string> ConfirmEmail(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:key"]);
        var email = string.Empty;
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
            }, out SecurityToken validatedToken);
            
            var encodeToken = (JwtSecurityToken)validatedToken;
            email = encodeToken.Claims.First(claim => claim.Type == "email").Value;
        }
        catch
        {
            throw new InvalidTokenEmailConfirmation();
        }

        var emailValidation = await _repository.Security.GetEmailValidation(email);
        _repository.Security.DeleteEmailValidation(emailValidation);
        
        var user = await _repository.User.GetUserByEmailAsync(email);
        user.CheckEmail();
        _repository.User.UpdateUser(user);

        await _repository.SaveAsync();

        return email;
    }

    private string GenerateJwtTokenForEmailConfirmation(string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, email)
            }),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Issuer"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    } 
}