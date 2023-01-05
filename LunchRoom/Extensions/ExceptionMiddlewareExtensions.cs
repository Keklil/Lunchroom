using Contracts;
using Domain.ErrorModel;
using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;
using Domain.Exceptions.AuthExceptions;
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
                            ValidationAppException => StatusCodes.Status422UnprocessableEntity,
                            DomainException => StatusCodes.Status422UnprocessableEntity,
                            _ => StatusCodes.Status500InternalServerError
                        };

                        logger.LogError($"{contextFeature.Error}");

                        if (contextFeature.Error is ValidationAppException exception)
                        {
                            await context.Response
                            .WriteAsync(JsonSerializer.Serialize(new { exception.Errors }));
                        } 
                        else if (contextFeature.Error is UserExistsException existsException)
                        {
                            await context.Response
                                .WriteAsync(JsonSerializer.Serialize(new AuthErrorResponse
                                {
                                    Code = AuthCodes.UserExists, 
                                    ExceptionMessage = existsException.Message
                                }));
                        }
                        else
                        {
                            await context.Response.WriteAsync(new ErrorDetails()
                            {
                                StatusCode = context.Response.StatusCode,
                                Message = contextFeature.Error.Message
                            }.ToString());
                        }
                    }
                });
            });
        }
    }
}
