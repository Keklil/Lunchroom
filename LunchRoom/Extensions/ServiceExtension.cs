using System.Reflection;
using System.Text;
using Application;
using Application.Behaviors;
using Application.Services.Mail;
using Contracts;
using Contracts.Repositories;
using Data;
using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using Infrastructure;
using Infrastructure.Logging;
using LunchRoom.Controllers.Infrastructure.Examples;
using MediatR;
using MediatR.Behaviors.Authorization.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Converters;
using NSwag;
using NSwag.Examples;
using NSwag.Generation.Processors.Security;

namespace LunchRoom.Extensions;

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

    public static void ConfigureLoggerService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<SeqConfig>()
            .Configure<IConfiguration>((seqOptions, configuration) =>
                configuration.GetSection("Seq").Bind(seqOptions))
            .ValidateDataAnnotations();

        services.AddSingleton<SeqConfig>(x =>
            x.GetRequiredService<IOptions<SeqConfig>>().Value);

        LoggerConfigurator.SetupSettings(configuration);
    }

    public static void ConfigureRepository(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<RepositoryContext>(options =>
        {
            options.UseNpgsql(config.GetConnectionString("DbConnection"),
                x =>
                {
                    x.UseNetTopologySuite();
                    x.MigrationsAssembly(nameof(Data));
                });
        });

        services.AddScoped<IRepositoryManager, RepositoryManager>();
    }

    public static void ConfigureMediatR(this IServiceCollection services)
    {
        services.AddMediatR(c =>
        {
            c.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly);
            c.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        services.AddMediatorAuthorization(Assembly.GetAssembly(typeof(AssemblyReference)));
        services.AddAuthorizersFromAssembly(Assembly.GetAssembly(typeof(AssemblyReference)));

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

    public static void AddControllersCustom(this IServiceCollection services)
    {
        services
            .AddControllers(options => options.ModelMetadataDetailsProviders.Add(new SuppressChildValidationMetadataProvider(typeof(Polygon))))
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new GeoJsonConverterFactory());
            });
        
        services.AddSingleton(NtsGeometryServices.Instance);
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddExampleProviders(typeof(UploadMenuExamples).Assembly);
        services.AddOpenApiDocument((settings, serviceProvider) =>
        {
            settings.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            });

            settings.OperationProcessors.Add(
                new AspNetCoreOperationSecurityScopeProcessor("JWT"));

            settings.GenerateEnumMappingDescription = true;

            settings.AddExamples(serviceProvider);
            settings.TypeMappers.Add(CustomOpenApiSchemas.GetPolygonSchema());
            settings.TypeMappers.Add(CustomOpenApiSchemas.GetPointSchema());
        });
    }
}