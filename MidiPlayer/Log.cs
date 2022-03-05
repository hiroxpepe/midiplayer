
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
            var eventInfo = new LogEventInfo(LogLevel.Fatal, _logger.Name, target);
            _logger.Log(typeof(Log), eventInfo);
        }

        public static void Error(string target) {
            var eventInfo = new LogEventInfo(LogLevel.Error, _logger.Name, target);
            _logger.Log(typeof(Log), eventInfo);
        }

        public static void Warn(string target) {
            var eventInfo = new LogEventInfo(LogLevel.Warn, _logger.Name, target);
            _logger.Log(typeof(Log), eventInfo);
        }

        public static void Info(string target) {
            var eventInfo = new LogEventInfo(LogLevel.Info, _logger.Name, target);
            _logger.Log(typeof(Log), eventInfo);
        }

        public static void Debug(string target) {
#if DEBUG
            var eventInfo = new LogEventInfo(LogLevel.Debug, _logger.Name, target);
            _logger.Log(typeof(Log), eventInfo);
#endif
        }

        public static void Trace(string target) {
#if DEBUG
            var eventInfo = new LogEventInfo(LogLevel.Trace, _logger.Name, target);
            _logger.Log(typeof(Log), eventInfo);
#endif
        }
    }
}
