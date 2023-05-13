using Application.Services.Auth;
using Application.Services.User;
using Contracts;
using Contracts.Security;
using Data.EntitiesConfiguration;
using Infrastructure.Extensions;
using Infrastructure.Logging;
using LunchRoom.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.HttpOverrides;
using Nominatim.API.Geocoders;
using Nominatim.API.Interfaces;
using Nominatim.API.Web;
using Services.Menu;
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
builder.Services.ConfigureHttpLogging();
//builder.Services.ConfigureInboxIdleClient();
builder.Services.AddScoped<EventsDispatcher>();
builder.Services.AddSingleton<IPlainTextParser, PlainTextParser>();
builder.Services.AddScoped<IOrdersReportService, OrdersReportService>();
builder.Services.AddScoped<IMailSender, Application.Services.Mail.Sender>();
builder.Services.AddScoped<IPushSender, Application.Services.Notifications.Sender>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IDataTableParser, DataTableParser>();
builder.Services.AddScoped<INominatimWebInterface, NominatimWebInterface>();
builder.Services.AddScoped<IForwardGeocoder, ForwardGeocoder>();
builder.Services.AddScoped<DbInitializer>();
builder.Services.AddHttpClient();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureReportingKitchenService();
builder.Services.ConfigureBackgroundTasksQueue();
//builder.Services.ConfigureHangfire(builder.Configuration);
builder.Services.AddControllersCustom();
ConfigurationExtension.ConfigureFirebaseAdmin();

var app = builder.Build();

app.UseHttpLogging();

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