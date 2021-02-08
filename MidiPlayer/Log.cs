
using NLog;

namespace MidiPlayer {
    /// <summary>
    /// ログ用ファサードクラス
    /// NOTE: NLog を使用
    /// </summary>
    public static class Log {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Fields

        static Logger logger = LogManager.GetCurrentClassLogger();

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public static Methods [verb]

        public static void Fatal(string target) {
            LogEventInfo _event = new LogEventInfo(LogLevel.Fatal, logger.Name, target);
            logger.Log(typeof(Log), _event);
        }

        public static void Error(string target) {
            LogEventInfo _event = new LogEventInfo(LogLevel.Error, logger.Name, target);
            logger.Log(typeof(Log), _event);
        }

        public static void Warn(string target) {
            LogEventInfo _event = new LogEventInfo(LogLevel.Warn, logger.Name, target);
            logger.Log(typeof(Log), _event);
        }

        public static void Info(string target) {
            LogEventInfo _event = new LogEventInfo(LogLevel.Info, logger.Name, target);
            logger.Log(typeof(Log), _event);
        }

        public static void Debug(string target) {
            LogEventInfo _event = new LogEventInfo(LogLevel.Debug, logger.Name, target);
            logger.Log(typeof(Log), _event);
        }

        public static void Trace(string target) {
            LogEventInfo _event = new LogEventInfo(LogLevel.Trace, logger.Name, target);
            logger.Log(typeof(Log), _event);
        }
    }
}