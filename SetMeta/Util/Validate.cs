using System;
using System.Collections.Generic;

namespace SetMeta.Util
{
    public static class Validate
    {
        private const string ArgumentIsInvalid = "Argument is invalid.";

        public static void Id(int id, string parameterName)
        {
            if (!ValidationHelper.Id(id))
                throw new ArgumentException(ArgumentIsInvalid, parameterName);
        }

        public static void Id(Guid id, string parameterName)
        {
            if (!ValidationHelper.Guid(id))
                throw new ArgumentException(ArgumentIsInvalid, parameterName);
        }

        public static void Positive(int value, string parameterName)
        {
            if (!ValidationHelper.Positive(value))
                throw new ArgumentException(ArgumentIsInvalid, parameterName);
        }

        public static void Positive(int? value, string parameterName)
        {
            if (!ValidationHelper.Positive(value))
                throw new ArgumentException(ArgumentIsInvalid, parameterName);
        }

        public static void ZeroOrPositive(int value, string parameterName)
        {
            if (!ValidationHelper.ZeroOrPositive(value))
                throw new ArgumentException(ArgumentIsInvalid, parameterName);
        }

        public static void String(string value, string parameterName)
        {
            if (!ValidationHelper.String(value))
                throw new ArgumentException(ArgumentIsInvalid, parameterName);
        }

        public static void String(string value, string parameterName, int maxLength)
        {
            if (!ValidationHelper.String(value, maxLength))
                throw new ArgumentException(ArgumentIsInvalid, parameterName);
        }

        public static void IdList(List<int> ids, string parameterName)
        {
            if (!ValidationHelper.IdList(ids))
                throw new ArgumentException(ArgumentIsInvalid, parameterName);
        }

        public static void IdArray(int[] ids, string parameterName)
        {
            if (!ValidationHelper.IdArray(ids))
                throw new ArgumentException(ArgumentIsInvalid, parameterName);
        }

        public static void NotEmptyIdList(List<int> ids, string parameterName)
        {
            if (!ValidationHelper.NotEmptyIdList(ids))
                throw new ArgumentException(ArgumentIsInvalid, parameterName);
        }

        public static void Enum<T>(T value, string parameterName)
            where T : struct
        {
            if (!ValidationHelper.Enum(value))
                throw new ArgumentException(ArgumentIsInvalid, parameterName);
        }

        public static void NotNullOrWhitespace(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(ArgumentIsInvalid, parameterName);
        }

        public static void NotNull<T>(T obj, string parameterName)
            where T : class
        {
            if (obj == null)
                throw new ArgumentNullException(parameterName);
        }

        public static void NotNull(int? id, string parameterName)
        {
            if (id == null)
                throw new ArgumentNullException(parameterName);
        }
    }
}