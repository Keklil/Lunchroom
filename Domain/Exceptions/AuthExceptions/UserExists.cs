﻿namespace Domain.Exceptions.AuthExceptions;

public class UserExistsException : AuthException
{
    public UserExistsException() : base("Пользователь с таким email уже существует.") {}
        
}