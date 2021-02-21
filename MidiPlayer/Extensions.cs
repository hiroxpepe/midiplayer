
using System;
using System.IO;

namespace MidiPlayer {
    /// <summary>
    /// common extension method
    /// </summary>
    internal static class Extensions {

        /// <summary>
        /// to directory name
        /// </summary>
        public static string ToDirectoryName(this string source) {
            return Path.GetDirectoryName(source);
        }

        /// <summary>
        /// to file name
        /// </summary>
        public static string ToFileName(this string source) {
            return Path.GetFileName(source);
        }

        /// <summary>
        /// bytes to megabytes.
        /// </summary>
        public static long ToMegabytes(this long source) {
            return source / (1024 * 1024);
        }

        /// <summary>
        /// returns TRUE if the string is not null or an empty string "".
        /// </summary>
        public static bool HasValue(this string source) {
            return !(source is null || source.Equals(""));
        }

        /// <summary>
        /// returns TRUE if IntPtr is IntPtr.Zero.
        /// </summary>
        public static bool IsZero(this IntPtr source) {
            return source == IntPtr.Zero;
        }
    }
}
