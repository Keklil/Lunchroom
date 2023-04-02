using System.ComponentModel.DataAnnotations;
using Serilog.Events;

namespace LoggerService;

public class SeqConfig
{
    [Required(ErrorMessage = "Пропущено или пустое необходимое поле конфигурации \"ServerUrl\"")]
    public string ServerUrl { get; set; }

    [Required(ErrorMessage = "Пропущено или пустое необходимое поле конфигурации \"ApiKey\"")]
    public string ApiKey { get; set; }

    [Required(ErrorMessage = "Пропущено или пустое необходимое поле конфигурации \"MinimumLogLevel\"")]
    public LogEventLevel MinimumLogLevel { get; set; }
}