using Application;
using Application.Behaviors;
using Contracts;
using Contracts.Security;
using Data.EntitiesConfiguration;
using LoggerService;
using LunchRoom.Extensions;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.HttpOverrides;
using Services.AuthService;
using Services.MailService;
using Services.OrdersReport;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureLoggerService(builder.Configuration);
builder.Host.ConfigureLogger();

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddMemoryCache();
builder.Services.ConfigureCors();

builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigurePostgreSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>),
    typeof(ValidationBehavior<,>));
builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));
builder.Services.ConfigureValidator();
//builder.Services.ConfigureInboxIdleClient();
builder.Services.AddSingleton<IMailParser, MailParser>();
builder.Services.AddScoped<IOrdersReportService, OrdersReportService>();
builder.Services.AddScoped<IMailSender, MailSender>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<DbInitializer>();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureReportingKitchenService();
//builder.Services.ConfigureHangfire(builder.Configuration);
builder.Services.AddControllers();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<ExceptionHandlerMiddleware>>();
app.ConfigureExceptionHandler(logger);

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetService<DbInitializer>();
    dbInitializer.Initialize();
    dbInitializer.SeedData();
}

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3(x => x.PersistAuthorization = true);
}
else
{
    app.UseHsts();
}

app.UseOpenApi();
app.UseSwaggerUi3(x => x.PersistAuthorization = true);
//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors("CorsPolicy");

app.Run();