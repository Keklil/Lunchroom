using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace LoggerService;

public static class LoggerConfigurator
{
    private static SeqConfig _config;
    
    public static void ConfigureLogger(this ConfigureHostBuilder host)
    {
        host.UseSerilog((context, configuration) => configuration
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.Seq(_config.ServerUrl, apiKey: _config.ApiKey, restrictedToMinimumLevel: _config.MinimumLogLevel));
    }

    public static void SetupSettings(IConfiguration configuration)
    {
        var seqSettings = configuration.GetSection("Seq").Get<SeqConfig>();
        _config = seqSettings!;
    }
}