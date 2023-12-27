
using Serilog;
using Serilog.Events;
using System;

namespace GameOnlineServer.Logging;

public class GameLogger : IGameLogger
{
    private readonly ILogger _logger;
    public GameLogger(){
        _logger = new LoggerConfiguration()
                .WriteTo.Console(
                    outputTemplate:
                    "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
               // .WriteTo.File("logging/log-.txt", LogEventLevel.Error, rollingInterval: RollingInterval.Day)
                .CreateLogger();
    }
    public void Error(string error, Exception exception)
    {
        _logger.Error(error,exception);
    }

    public void Error(string error)
    {
        _logger.Error(error);
    }

    public void Info(string info)
    {
       _logger.Information(info);
    }

    public void Print(string msg)
    {
       _logger.Information(messageTemplate:$">>>>> {msg}");
    }

    public void Warning(string msg, Exception exception = null)
    {
        _logger.Warning(msg,exception);
    }
}
