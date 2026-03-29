namespace GlideBuy.Core
{
    public static class CommonHelper
    {
        public static string EnsureNotNull(string str)
        {
            return str ?? string.Empty;
        }

        // Ensure the string doesn't exceed maximum allowed length.
        // The postfix could be added to the string if it is shortened.
        // For example, the caller could supply a three dots '...' to append to each shortened string.
        public static string EnsureMaximumLength(string str, int maxLength, string? postfix = null)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            if (str.Length <= maxLength)
                return str;

            // The string is longer than maxLength, therefore it will be shortened,
            // and a postfix might be appended to it.

            var postfixLength = postfix?.Length ?? 0;

            var result = str[0..(maxLength - postfixLength)];
            if (!string.IsNullOrEmpty(postfix))
                result += postfix;

            return result;
        }
    }
}
