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

using System;
using System.IO;
using System.Text;

namespace MidiPlayer {
    /// <summary>
    /// common extension method
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public static class Extensions {
#nullable enable

        /// <summary>
        /// to memory stream
        /// </summary>
        /// <param name="source">the string to convert.</param>
        /// <returns>a MemoryStream containing the UTF-8 encoded bytes of the string.</returns>
        public static MemoryStream ToMemoryStream(this string source) {
            return new MemoryStream(buffer: Encoding.UTF8.GetBytes(source));
        }

        /// <summary>
        /// to directory name
        /// </summary>
        /// <param name="source">the full file path.</param>
        /// <returns>the directory portion of the path.</returns>
        public static string ToDirectoryName(this string source) {
            return Path.GetDirectoryName(path: source);
        }

        /// <summary>
        /// to file name
        /// </summary>
        /// <param name="source">the full file path.</param>
        /// <returns>the file name portion of the path.</returns>
        public static string ToFileName(this string source) {
            return Path.GetFileName(path: source);
        }

        /// <summary>
        /// bytes to megabytes.
        /// </summary>
        /// <param name="source">the value in bytes.</param>
        /// <returns>the value converted to megabytes.</returns>
        public static long ToMegabytes(this long source) {
            return source / (1024 * 1024);
        }

        /// <summary>
        /// returns true if the string is not null or an empty string "" or "undefined".
        /// </summary>
        /// <param name="source">the string to check.</param>
        /// <returns>true when the string has a usable value; false otherwise.</returns>
        public static bool HasValue(this string source) {
            return !(source is null || source.Equals(string.Empty) || source.Equals("undefined"));
        }

        /// <summary>
        /// returns true if IntPtr is IntPtr.Zero.
        /// </summary>
        /// <param name="source">the pointer to check.</param>
        /// <returns>true when the pointer equals IntPtr.Zero; false otherwise.</returns>
        public static bool IsZero(this IntPtr source) {
            return source == IntPtr.Zero;
        }
    }
}
