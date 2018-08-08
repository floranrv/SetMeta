using System;
using System.Globalization;
using SetMeta.Abstract;
using SetMeta.Util;

namespace SetMeta.Impl
{
    internal class OptionValueConverter<T>
        : IOptionValueConverter<T>
    {
        public T GetValue(string value)
        {
            return DataConversion.Convert<T>(value);
        }

        public string GetStringValue(T value)
        {
            if (value == null)
                return null;

            return GetStringValue(value, CultureInfo.InvariantCulture);
        }

        public string GetStringValue(T value, IFormatProvider formatProvider)
        {
            if (formatProvider == null) throw new ArgumentNullException(nameof(formatProvider));
            if (value == null)
                return null;

            return Convert.ToString(value, formatProvider);
        }
    }
}