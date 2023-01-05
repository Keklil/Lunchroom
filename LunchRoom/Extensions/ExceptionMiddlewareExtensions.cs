using Contracts;
using Domain.ErrorModel;
using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;
using Domain.Exceptions.AuthExceptions;
using Domain.Exceptions.GroupExceptions;
using LunchRoom.Controllers.Infrastructure;

namespace LunchRoom.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this WebApplication app, ILoggerManager logger)
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
                            NotFoundException => StatusCodes.Status404NotFound,
                            BadRequestException => StatusCodes.Status400BadRequest,
                            ValidationAppException => StatusCodes.Status400BadRequest,
                            DomainException => StatusCodes.Status400BadRequest,
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
                            
                            case UserAlreadyInGroupException userInGroupException:
                                await context.Response
                                    .WriteAsync(JsonSerializer.Serialize(new GroupErrorResponse()
                                    {
                                        Code = GroupErrorResponse.ErrorCodes.UserIsMember,
                                        ExceptionMessage = userInGroupException.Message
                                    }));
                                break;
                            
                            default:
                                await context.Response
                                    .WriteAsync(new ErrorDetails()
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
}
