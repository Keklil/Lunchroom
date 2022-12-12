using Contracts;
using Contracts.Repositories;
using Contracts.Security;
using Domain.DataTransferObjects.User;
using Domain.Models;
using Microsoft.Extensions.Configuration;

namespace Services.AuthService;

public class RegistrationService : IRegistrationService
{
    private readonly IRepositoryManager _repository;
    private readonly IConfiguration _configuration;
    private readonly IMailSender _mailSender;
    private readonly ITokenService _tokenService;
    
    private string subject = "Подтверждение почты";
    private string hostLink = "http://localhost:5097/EmailConfirmation?token=";
    
    public RegistrationService(
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
    
    public async Task<User> RegisterAdmin(UserRegisterDto user)
    {
        var userEntity = await _repository.User.GetUserByEmailAsync(user.email);
        if (userEntity is not null)
            return userEntity;

        return userEntity;
    }
    
    public async Task<User> RegisterUser(UserRegisterDto user)
    {
        return await _repository.User.GetUserByEmailAsync(user.email);
    }
}