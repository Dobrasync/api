namespace Dobrasync.Api.BusinessLogic.Services.Core.Logging;

public interface ILoggingService
{
    void LogDebug(string msg);
    void LogInfo(string msg);
    void LogWarn(string msg);
    void LogError(string msg);
    void LogFatal(string msg);
}