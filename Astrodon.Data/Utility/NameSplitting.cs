#region Usings

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

#endregion

namespace Astradon.Data.Utility
{
    public static class NameSplitting
    {
        /// <summary>
        ///     Splits the camel case.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string SplitCamelCase(string value)
        {
            if (value == null)
                return null;
            return Regex.Replace(Regex.Replace(value, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
        }

        public static string SplitCamelCase(object value)
        {
            if (value == null)
                return null;
            return SplitCamelCase(value.ToString());
        }

        /// <summary>
        ///     Returns part of a string up to the specified number of characters, while maintaining full words
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length">Maximum characters to be returned</param>
        /// <returns>String</returns>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "s")]
        public static string Chop(this string s, int length)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException(s);

            var words = s.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();

            foreach (var word in words.Where(word => sb.ToString().Length < length))
                sb.Append(word + " ");

            return sb.ToString().TrimEnd(' ') + "...";
        }

        public static string ToTitleCase(string s)
        {
            var result = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());
            return result.Replace("  ", " ").Replace("  ", " ");
        }
    }
}