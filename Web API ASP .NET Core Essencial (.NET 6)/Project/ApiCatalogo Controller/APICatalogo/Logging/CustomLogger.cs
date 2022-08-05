namespace APICatalogo.Logging
{
    public class CustomLogger : ILogger
    {
        readonly string loggerName;
        readonly CustomLoggerProviderConfiguration loggerCofig;
        
        public CustomLogger(string name, CustomLoggerProviderConfiguration config)
        {
            loggerName = name;
            loggerCofig = config;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string message = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";
            EscreverTextoNoArquivo(message);
        }

        private void EscreverTextoNoArquivo(string message)
        {
            string camihoArquivoLog = @"c:\dados\log\fellipe_log.txt";
            using (StreamWriter streamWriter = new StreamWriter(camihoArquivoLog, true))
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
