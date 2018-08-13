using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SetMeta.Util
{
    public static class ValidationHelper
    {
        public static bool Id(int id)
            => id > 0;

        public static bool Id(int? id)
            => id == null || id > 0;

        public static bool ZeroOrPositive(int value)
            => value >= 0;

        public static bool ZeroOrPositive(int? value)
            => value == null || value >= 0;

        public static bool Positive(int value)
            => value > 0;

        public static bool Positive(int? value)
            => value == null || value > 0;

        public static bool String(string value)
            => String(value, 4000);

        public static bool String(string value, int maxLength)
            => !string.IsNullOrWhiteSpace(value) && value.Length <= maxLength;

        public static bool StringOrEmpty(string value, int maxLength)
            => value == null || String(value, maxLength);

        public static bool Guid(Guid guid)
            => guid != System.Guid.Empty;

        public static bool Guid(Guid? guid)
            => guid != System.Guid.Empty;

        // regex pattern is taken from https://msdn.microsoft.com/en-us/library/01escwtf(v=vs.110).aspx article
        public static bool Email(string email)
            => String(email) && IsMatchRegex(email,
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");

        public static bool EmailOrEmpty(string email)
            => string.IsNullOrEmpty(email) || Email(email);

        public static bool IdList(List<int> ids)
            => ids != null && ids.All(Id);

        public static bool IdArray(int[] ids)
            => ids != null && ids.All(Id);

        public static bool UniqueIdList(List<int> list)
            => IdList(list) && list.Count == list.Distinct().Count();

        public static bool NotEmptyIdList(List<int> ids)
            => IdList(ids) && ids.Any();

        public static bool StringList(List<string> list)
            => list != null && list.All(String);

        public static bool UniqueStringList(List<string> list)
            => StringList(list) && list.Count == list.Select(s => s.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).Count();

        public static bool Enum<T>(T value) where T : struct
            => System.Enum.IsDefined(typeof(T), value);

        public static bool Enum<T>(T? value) where T : struct
            => value == null || System.Enum.IsDefined(typeof(T), value.Value);

        private static bool IsMatchRegex(string str, string expr)
        {
            try
            {
                return Regex.IsMatch(str, expr, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}