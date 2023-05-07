﻿using System.ComponentModel;
using System.Text.Json;
using Application.Authorization.Exceptions;
using Domain.ErrorModel;
using Domain.Exceptions;
using Domain.Exceptions.AuthExceptions;
using Domain.Exceptions.Base;
using Domain.Exceptions.GroupExceptions;
using Domain.Exceptions.KitchenExceptions;
using LunchRoom.Controllers.Infrastructure;
using MediatR.Behaviors.Authorization.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace LunchRoom.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this WebApplication app, ILogger logger)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    context.Response.StatusCode = contextFeature.Error switch
                    {
                        NotFoundException => StatusCodes.Status400BadRequest,
                        BadRequestException => StatusCodes.Status400BadRequest,
                        ValidationAppException => StatusCodes.Status400BadRequest,
                        DomainException => StatusCodes.Status400BadRequest,
                        InvalidEnumArgumentException => StatusCodes.Status400BadRequest,
                        UnauthorizedException => StatusCodes.Status403Forbidden,
                        _ => StatusCodes.Status500InternalServerError
                    };

                    switch (contextFeature.Error is StructuredException)
                    {
                        case true:
                            var error = contextFeature.Error as StructuredException;
                            logger.LogError(contextFeature.Error, error.MessageTemplate, error.Args);
                            break;
                        case false:
                            logger.LogError(contextFeature.Error, contextFeature.Error.Message);
                            break;
                    }
                    
                    switch (contextFeature.Error)
                    {
                        case ValidationAppException exception:
                            await context.Response
                                .WriteAsync(JsonSerializer.Serialize(new { exception.Errors }));
                            break;

                        case UserExistsException existsException:
                            await context.Response
                                .WriteAsync(JsonSerializer.Serialize(new AuthErrorResponse
                                {
                                    Code = AuthErrorResponse.ErrorCodes.UserExists,
                                    ExceptionMessage = existsException.Message
                                }));
                            break;

                        case WrongUserCredentialsException wrongUserCredentialsException:
                            await context.Response
                                .WriteAsync(JsonSerializer.Serialize(new AuthErrorResponse
                                {
                                    Code = AuthErrorResponse.ErrorCodes.WrongCredentials,
                                    ExceptionMessage = wrongUserCredentialsException.Message
                                }));
                            break;

                        case UnconfirmedEmailException unconfirmedEmailException:
                            await context.Response
                                .WriteAsync(JsonSerializer.Serialize(new AuthErrorResponse
                                {
                                    Code = AuthErrorResponse.ErrorCodes.UnconfirmedEmail,
                                    ExceptionMessage = unconfirmedEmailException.Message
                                }));
                            break;

                        case UserAlreadyInGroupException userInGroupException:
                            await context.Response
                                .WriteAsync(JsonSerializer.Serialize(new GroupErrorResponse
                                {
                                    Code = GroupErrorResponse.ErrorCodes.UserIsMember,
                                    ExceptionMessage = userInGroupException.Message
                                }));
                            break;

                        case AttemptCreateGroupByNonCustomerException attemptCreateGroupByNonAdminException:
                            await context.Response
                                .WriteAsync(JsonSerializer.Serialize(new GroupErrorResponse
                                {
                                    Code = GroupErrorResponse.ErrorCodes.AttemptCreateGroupByNonAdmin,
                                    ExceptionMessage = attemptCreateGroupByNonAdminException.Message
                                }));
                            break;
                        
                        case AttemptCreateKitchenByNonKitchenOperator attemptCreateKitchenByNonKitchenOperator:
                            await context.Response
                                .WriteAsync(JsonSerializer.Serialize(new KitchenErrorResponse()
                                {
                                    Code = KitchenErrorResponse.ErrorCodes.AttemptCreateGroupByNonKitchenOperator,
                                    ExceptionMessage = attemptCreateKitchenByNonKitchenOperator.Message
                                }));
                            break;
                        
                        case UserAlreadyInKitchenException attemptCreateKitchenByNonKitchenOperator:
                            await context.Response
                                .WriteAsync(JsonSerializer.Serialize(new KitchenErrorResponse()
                                {
                                    Code = KitchenErrorResponse.ErrorCodes.UserIsMember,
                                    ExceptionMessage = attemptCreateKitchenByNonKitchenOperator.Message
                                }));
                            break;

                        default:
                            await context.Response
                                .WriteAsync(new ErrorDetails
                                {
                                    StatusCode = context.Response.StatusCode,
                                    ExceptionMessage = contextFeature.Error.Message
                                }.ToString());
                            break;
                    }
                }
            });
        });
    }
}