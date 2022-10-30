using System.Reflection;
using System.Text;
using Contracts;
using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using LoggerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using Quartz;
using Repository;
using Services.MailService;
using Services.OrdersReport;

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
                    x => x.MigrationsAssembly("LunchRoom")));
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
                services => services.GetRequiredService<InboxIdleService>());
        }

        public static void ConfigureReportingKitchenService(this IServiceCollection services)
        {
            services.AddSingleton<ReportingKitchenService>();
            services.AddHostedService(
                services => services.GetRequiredService<ReportingKitchenService>());
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
            services.AddOpenApiDocument(document =>
            {
                document.AddSecurity("JWT", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}."
                });
 
                document.OperationProcessors.Add(
                    new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });
            
            /*
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "LunchRoom API"
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            });*/
        }
    }
}
