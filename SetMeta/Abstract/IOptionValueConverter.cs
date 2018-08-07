using System;

namespace SetMeta.Abstract
{
    public interface IOptionValueConverter<T>
    {
        T GetValue(string value);
        string GetStringValue(T value);
        string GetStringValue(T value, IFormatProvider formatProvider);
    }
}