
using NLog;

namespace MidiPlayer {
    /// <summary>
    /// Facade class for log
    /// NOTE: using NLog
    /// </summary>
    public static class Log {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Fields [nouns, noun phrases]

        static Logger _logger = LogManager.GetCurrentClassLogger();

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public static Methods [verb, verb phrases]

        public static void Fatal(string target) {
            LogEventInfo eventInfo = new LogEventInfo(LogLevel.Fatal, _logger.Name, target);
            _logger.Log(typeof(Log), eventInfo);
        }

        public static void Error(string target) {
            LogEventInfo eventInfo = new LogEventInfo(LogLevel.Error, _logger.Name, target);
            _logger.Log(typeof(Log), eventInfo);
        }

        public static void Warn(string target) {
            LogEventInfo eventInfo = new LogEventInfo(LogLevel.Warn, _logger.Name, target);
            _logger.Log(typeof(Log), eventInfo);
        }

        public static void Info(string target) {
            LogEventInfo eventInfo = new LogEventInfo(LogLevel.Info, _logger.Name, target);
            _logger.Log(typeof(Log), eventInfo);
        }

        public static void Debug(string target) {
            LogEventInfo eventInfo = new LogEventInfo(LogLevel.Debug, _logger.Name, target);
            _logger.Log(typeof(Log), eventInfo);
        }

        public static void Trace(string target) {
            LogEventInfo eventInfo = new LogEventInfo(LogLevel.Trace, _logger.Name, target);
            _logger.Log(typeof(Log), eventInfo);
        }
    }
}