using System.Reflection;
using System.Text;
using Contracts;
using Contracts.Repositories;
using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using LoggerService;
using LunchRoom.Controllers.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Examples;
using NSwag.Generation.Processors.Security;
using Quartz;
using Repository;
using Services.MailService;
using Services.OrdersReport;
using ZymLabs.NSwag.FluentValidation;

namespace LunchRoom.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(option =>
            {
                option.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void ConfigureRepositoryManager(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }
    
        public static void ConfigurePostgreSqlContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<RepositoryContext>(options => 
                options.UseNpgsql(config.GetConnectionString("DbConnection"),
                    x => x.MigrationsAssembly(nameof(Repository))));
        }

        public static void ConfigureValidator(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(Application.AssemblyReference).Assembly);
            ValidatorOptions.Global.LanguageManager.Enabled = false;
        }

        public static void ConfigureInboxIdleClient(this IServiceCollection services)
        {
            services.AddScoped<IMailIdleClient, MailIdleClient>();
            services.AddSingleton<InboxIdleService>();
            services.AddHostedService(
                serviceProvider => serviceProvider.GetRequiredService<InboxIdleService>());
        }

        public static void ConfigureReportingKitchenService(this IServiceCollection services)
        {
            // services.AddSingleton<ReportingKitchenService>();
            // services.AddHostedService(
            //     serviceProvider => serviceProvider.GetRequiredService<ReportingKitchenService>());
        }
        

        public static void ConfigureHangfire(this IServiceCollection services, IConfiguration config)
        { 
            services.AddHangfire(x =>
                x.UsePostgreSqlStorage(config.GetConnectionString("DbConnection")));

            services.AddHangfireServer();
        }

        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
        
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddExampleProviders(typeof(UploadMenuExamples).Assembly);
            services.AddOpenApiDocument((settings, serviceProvider) =>
            {
                settings.AddSecurity("JWT", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}.",
                });
 
                settings.OperationProcessors.Add(
                    new AspNetCoreOperationSecurityScopeProcessor("JWT"));

                settings.GenerateEnumMappingDescription = true;
                
                settings.AddExamples(serviceProvider);
            });
        }
    }
}
