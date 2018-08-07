using SetMeta.Util;

namespace SetMeta.Abstract
{
    public interface ITryParser
    {
        /// <summary>
        /// Метод попытки парсинга строки
        /// </summary>
        /// <param name="input"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool TryParse(string input, out object value);
    }

    /// <summary>
    /// Стандартный TryParser
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TryParser<T>
        : ITryParser
    {
        private readonly TryParseMethod<T> _parsingMethod;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="parsingMethod"></param>
        public TryParser(TryParseMethod<T> parsingMethod)
        {
            _parsingMethod = parsingMethod;
        }

        public bool TryParse(string input, out object value)
        {
            T parsedOutput;
            bool success = _parsingMethod(input, out parsedOutput);
            value = parsedOutput;

            return success;
        }
    }
}