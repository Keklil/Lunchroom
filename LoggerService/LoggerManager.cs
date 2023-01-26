using Contracts;
using Microsoft.Extensions.Options;
using Serilog;

namespace LoggerService
{
    public class LoggerManager : ILoggerManager
    {
        private readonly ILogger _logger;
        private readonly SeqConfig _config;

        public LoggerManager(IOptions<SeqConfig> config)
        {
            _config = config.Value;
            
            Serilog.Debugging.SelfLog.Out = Console.Error;
            
            _logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.Seq(_config.ServerUrl, apiKey: _config.ApiKey)
                .CreateLogger();
        }
        
        public void LogDebug(string message) => _logger.Debug(message);
        public void LogError(string message) => _logger.Error(message);
        public void LogInfo(string message) => _logger.Information(message);
        public void LogWarning(string message) => _logger.Warning(message);
    }

}