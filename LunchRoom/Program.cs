using Contracts;
using Contracts.Security;
using Data.EntitiesConfiguration;
using Identity.Services;
using LoggerService;
using LunchRoom.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.HttpOverrides;
using Services;
using Services.AuthService;
using Services.MailService;
using Services.OrdersReport;

System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureLoggerService(builder.Configuration);
builder.Host.ConfigureLogger();

builder.Configuration.AddEnvironmentVariables();

builder.Services.ConfigureCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureRepository(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureMediatR();
//builder.Services.ConfigureInboxIdleClient();
builder.Services.AddSingleton<IMailParser, MailParser>();
builder.Services.AddScoped<IOrdersReportService, OrdersReportService>();
builder.Services.AddScoped<IMailSender, MailSender>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IMenuImportService, MenuImportService>();
builder.Services.AddScoped<DbInitializer>();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureReportingKitchenService();
//builder.Services.ConfigureHangfire(builder.Configuration);
builder.Services.AddControllersCustom();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<ExceptionHandlerMiddleware>>();
app.ConfigureExceptionHandler(logger);

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
    dbInitializer.Initialize();
    dbInitializer.SeedData();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseOpenApi();
app.UseSwaggerUi3(x => x.PersistAuthorization = true);

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