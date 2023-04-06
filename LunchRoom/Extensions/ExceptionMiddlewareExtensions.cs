using System.Text.Json;
using Domain.ErrorModel;
using Domain.Exceptions;
using Domain.Exceptions.AuthExceptions;
using Domain.Exceptions.GroupExceptions;
using Identity.Exceptions;
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
                        UnauthorizedException => StatusCodes.Status403Forbidden,
                        _ => StatusCodes.Status500InternalServerError
                    };

                    logger.LogError($"{contextFeature.Error}");

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

                        case AttemptCreateGroupByNonAdminException attemptCreateGroupByNonAdminException:
                            await context.Response
                                .WriteAsync(JsonSerializer.Serialize(new GroupErrorResponse
                                {
                                    Code = GroupErrorResponse.ErrorCodes.AttemptCreateGroupByNonAdmin,
                                    ExceptionMessage = attemptCreateGroupByNonAdminException.Message
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