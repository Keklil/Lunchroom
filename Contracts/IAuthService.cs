﻿namespace Contracts;

public interface IAuthService
{
    Task<string> Auth(string email);
    Task<string> ConfirmEmail(string token);
}