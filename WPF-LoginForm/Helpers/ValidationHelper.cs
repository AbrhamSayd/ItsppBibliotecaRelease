using System;
using System.Net.Mail;

namespace WPFBiblioteca.Helpers;

public class ValidationHelper
{
    internal static class Email
    {
        internal static bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith(".")) return false; // suggested by @TK-421
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
    }

    internal static class TryConvert
    {
        /// <summary>
        ///     Returns the integer result of parsing a string, or null.
        /// </summary>
        internal static int? ToNullableInt32(string toParse)
        {
            int result;
            if (int.TryParse(toParse, out result)) return result;
            return null;
        }

        internal static long? ToNullableLong(string toParse)
        {
            long result;
            if (long.TryParse(toParse, out result)) return result;
            return null;
        }

        internal static DateTime? ToNullableDateTime(string toParse)
        {
            DateTime result;
            if (DateTime.TryParse(toParse, out result)) return result;
            return null;
        }

        /// <summary>
        ///     Returns the integer result of parsing a string,
        ///     or the supplied failure value if the parse fails.
        /// </summary>
        internal static int ToInt32(string toParse, int toReturnOnFailure)
        {
            // The nullable-result method sets up for a coalesce operator.
            return ToNullableInt32(toParse) ?? toReturnOnFailure;
        }

        internal static long ToLong(string toParse, int toReturnOnFailure)
        {
            return ToNullableLong(toParse) ?? toReturnOnFailure;
        }

        internal static DateTime ToDateTime(string toParse, DateTime toReturnOnFailure)
        {
            return ToNullableDateTime(toParse) ?? toReturnOnFailure;
        }
    }
}