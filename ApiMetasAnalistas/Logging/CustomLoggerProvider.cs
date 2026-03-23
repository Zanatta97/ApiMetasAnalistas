using System.Collections.Concurrent;

namespace ApiMetasAnalistas.Logging
{
    public class CustomLoggerProvider : ILoggerProvider
    {
        readonly CustomLoggerProviderConfiguration _configuration;
        readonly ConcurrentDictionary<string, CustomLogger> _loggers = 
            new ConcurrentDictionary<string, CustomLogger>();

        public CustomLoggerProvider(CustomLoggerProviderConfiguration configuration)
        {
            _configuration = configuration;
        }


        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new CustomLogger(name, _configuration));
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}
