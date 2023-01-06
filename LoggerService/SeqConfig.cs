namespace LoggerService;

public class SeqConfig
{
    public string ServerUrl { get; set; }
    public string ApiKey { get; set; }
    public string MinimumLevel { get; set; }
    public LevelOverrideConfig LevelOverride { get; set; }

    public class LevelOverrideConfig
    {
        public string Microsoft { get; set; }
    }
}