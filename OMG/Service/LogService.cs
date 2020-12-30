using NLog;

namespace OMG.Service
{
    public static class LogService
    {
        public static Logger Logger { get; } = LogManager.GetCurrentClassLogger();
    }
}
