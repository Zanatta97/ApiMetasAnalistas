
namespace ApiMetasAnalistas.Logging
{
    public class CustomLogger : ILogger
    {
        readonly string _name;
        readonly CustomLoggerProviderConfiguration _config;

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

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            string message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {logLevel} - {eventId.Id} - {_name}: {formatter(state, exception)}";
            WriteMessageInFile(message);
        }

        private void WriteMessageInFile(string message)
        {
            string logFilePath = _config.LogPath + _config.LogFile;

            using (StreamWriter streamWriter = new StreamWriter(logFilePath, true))
            {
                try
                {
                    streamWriter.WriteLine(message);
                    streamWriter.Close();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}
