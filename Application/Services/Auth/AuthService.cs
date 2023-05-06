using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Authorization.Exceptions;
using Contracts;
using Contracts.Repositories;
using Contracts.Security;
using Domain.Exceptions;
using Domain.Models;
using Domain.SecurityModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.DataTransferObjects.User;

namespace Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IMailSender _mailSender;
    private readonly IRepositoryManager _repository;
    private readonly ITokenService _tokenService;
    private readonly string hostLink = "http://localhost:5097/EmailConfirmation?token=";
    private readonly string subject = "Подтверждение почты";

    public async Task<string> Auth(UserLogin login)
    {
        var user = await _repository.User.GetUserByEmailAsync(login.Email);
        if (user is null) throw new WrongUserCredentialsException();

        if (user.IsEmailChecked is false)
        {
            throw new UnconfirmedEmailException();
        }

        var passMatch = user.Password == login.Password;
        var token = passMatch
            ? await _tokenService.Generate(user)
            : throw new WrongUserCredentialsException();
        return token;
    }

    public async Task<string> ConfirmEmail(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:key"]);
        string email;
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            }, out var validatedToken);

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

    public async Task<User> RegisterUser(UserRegisterDto user)
    {
        var userEntity = await _repository.User.GetUserByEmailAsync(user.Email);
        if (userEntity is not null) throw new UserExistsException();

        var newUser = user.RoleDto switch
        {
            RoleDto.Customer => User.CreateCustomer(user.Email, user.Password),
            RoleDto.KitchenOperator => User.CreateKitchenOperator(user.Email, user.Password),
            _ => throw new ArgumentOutOfRangeException()
        };
        _repository.User.CreateUser(newUser);
        await SendConfirmationEmail(user.Email);
        await _repository.SaveAsync();
        return newUser;
    }

    private async Task SendConfirmationEmail(string email)
    {
        var token = GenerateJwtTokenForEmailConfirmation(email);
        var link = hostLink + token;
        string body;
        using (var reader = new StreamReader(Directory.GetCurrentDirectory() + "/Html/email.html"))
        {
            body = await reader.ReadToEndAsync();
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

    private string GenerateJwtTokenForEmailConfirmation(string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, email)
            }),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Issuer"],
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

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
}