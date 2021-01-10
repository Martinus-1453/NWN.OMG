using NLog;

namespace OMG.Service
{
    public static class Log
    {
        public static Logger Logger { get; } = LogManager.GetCurrentClassLogger();
    }
}