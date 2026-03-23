
namespace ApiMetasAnalistas.Logging
{
    public class CustomLogger : ILogger
    {
        readonly string _name;
        readonly CustomLoggerProviderConfiguration _config;
        private static readonly object _lockObj = new object();

        public CustomLogger(string name, CustomLoggerProviderConfiguration config)
        {
            _name = name;
            _config = config;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _config.LogLevel;
        }

        public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            string message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {logLevel} - {eventId.Id} - {_name}: {formatter(state, exception)}";
            await WriteMessageInFile(message);
        }

        private async Task WriteMessageInFile(string message)
        {
            string logFilePath = _config.LogPath + _config.LogFile;

            //Pode afetar o desempenho em cenários de alta concorrência, mas garante a integridade dos dados no arquivo de log.
            lock (_lockObj) // Garante que somente 1 thread escreva no arquivo de log por vez
            {
                try
                {
                    using var stream = new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    using var writer = new StreamWriter(stream);
                    writer.WriteLine(message);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}
