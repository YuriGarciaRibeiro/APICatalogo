namespace APICatalogo.Logging;

public class CustomerLogger : ILogger
{
    readonly string loggerName;
    readonly CustomLoggerProviderConfiguration loggerConfig;

    public CustomerLogger(string name, CustomLoggerProviderConfiguration config)
    {
        loggerName = name;
        loggerConfig = config;
    }

    public IDisposable? BeginScope<TState>(TState state)
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel == loggerConfig.LogLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception exception, Func<TState, Exception, string> formatter)
    {
        string mensagem = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";

        EscreverTextoNoArquivo(mensagem);
    }

    private void EscreverTextoNoArquivo(string mensagem)
{
    string caminhoDiretorio = @"c:\dados\log\";
    string caminhoArquivoLog = Path.Combine(caminhoDiretorio, "Log.txt");

    // Verifica se o diretório existe, caso contrário, cria-o
    if (!Directory.Exists(caminhoDiretorio))
    {
        Directory.CreateDirectory(caminhoDiretorio);
    }

    // Verifica se o arquivo de log existe, caso contrário, cria-o
    if (!File.Exists(caminhoArquivoLog))
    {
        File.Create(caminhoArquivoLog).Close();
    }

    // Escreve no arquivo de log
    using (StreamWriter streamWriter = new StreamWriter(caminhoArquivoLog, true))
    {
        try
        {
            streamWriter.WriteLine(mensagem);
        }
        catch (Exception)
        {
            throw;
        }
    }
}

}
