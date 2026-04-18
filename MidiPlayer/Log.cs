/*
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 2 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using NLog;

namespace MidiPlayer {
    /// <summary>
    /// Facade class for log
    /// NOTE: using NLog
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public static class Log {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Fields [nouns, noun phrases]

        /// <summary>
        /// the NLog logger instance for this class.
        /// </summary>
        static Logger _logger = LogManager.GetCurrentClassLogger();

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public static Methods [verb, verb phrases]

        /// <summary>
        /// logs a fatal-level message.
        /// </summary>
        /// <param name="target">the message to log.</param>
        public static void Fatal(string target) {
            var eventInfo = new LogEventInfo(LogLevel.Fatal, _logger.Name, target);
            _logger.Log(typeof(Log), eventInfo);
        }

        /// <summary>
        /// logs an error-level message.
        /// </summary>
        /// <param name="target">the message to log.</param>
        public static void Error(string target) {
            var eventInfo = new LogEventInfo(LogLevel.Error, _logger.Name, target);
            _logger.Log(typeof(Log), eventInfo);
        }

        /// <summary>
        /// logs a warning-level message.
        /// </summary>
        /// <param name="target">the message to log.</param>
        public static void Warn(string target) {
            var eventInfo = new LogEventInfo(LogLevel.Warn, _logger.Name, target);
            _logger.Log(typeof(Log), eventInfo);
        }

        /// <summary>
        /// logs an info-level message.
        /// </summary>
        /// <param name="target">the message to log.</param>
        public static void Info(string target) {
            var eventInfo = new LogEventInfo(LogLevel.Info, _logger.Name, target);
            _logger.Log(typeof(Log), eventInfo);
        }

        /// <summary>
        /// logs a debug-level message; only active in DEBUG builds.
        /// </summary>
        /// <param name="target">the message to log.</param>
        public static void Debug(string target) {
#if DEBUG
            var eventInfo = new LogEventInfo(LogLevel.Debug, _logger.Name, target);
            _logger.Log(typeof(Log), eventInfo);
#endif
        }

        /// <summary>
        /// logs a trace-level message; only active in DEBUG builds.
        /// </summary>
        /// <param name="target">the message to log.</param>
        public static void Trace(string target) {
#if DEBUG
            var eventInfo = new LogEventInfo(LogLevel.Trace, _logger.Name, target);
            _logger.Log(typeof(Log), eventInfo);
#endif
        }
    }
}
