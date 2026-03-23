namespace ApiMetasAnalistas.Logging
{
    public class CustomLoggerProviderConfiguration
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Warning;
        public int EventId { get; set; } = 0;
        public string? LogPath { get; set; } = @$"c:\temp\";
        public string? LogFile { get; set; } = @$"Log_ApiMetasAnalistas_{DateTime.Now:yyyyMMdd}.log";
    }
}
