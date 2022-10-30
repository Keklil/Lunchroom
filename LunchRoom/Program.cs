using Microsoft.AspNetCore.HttpOverrides;
using LunchRoom.Extensions;
using Contracts;
using MediatR;
using Application.Behaviors;
using Hangfire;
using Services.AuthService;
using Services.MailService;
using Services.OrdersReport;
using Repository.EntitiyConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
//builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.ConfigureCors();
builder.Services.ConfigureLoggerService();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.ConfigurePostgreSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>),
typeof(ValidationBehavior<,>));
builder.Services.AddMediatR(typeof(Application.AssemblyReference).Assembly);
builder.Services.ConfigureValidator();
builder.Services.ConfigureInboxIdleClient();
builder.Services.AddSingleton<IMailParser,MailParser>();
builder.Services.AddScoped<IOrdersReportService, OrdersReportService>();
builder.Services.AddScoped<IMailSender, MailSender>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<DbInitializer>();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureReportingKitchenService();
//builder.Services.ConfigureHangfire(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILoggerManager>();
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
    app.UseSwaggerUi3();
}
else
{
    app.UseHsts();
}
app.UseOpenApi();
app.UseSwaggerUi3();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

//app.UseHangfireDashboard();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors("CorsPolicy");

app.Run();
