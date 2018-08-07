namespace SetMeta.Util
{
    public interface ITryParser
    {
        bool TryParse(string input, out object value);
    }

    public class TryParser<T>
        : ITryParser
    {
        private readonly TryParseMethod<T> _parsingMethod;

        public TryParser(TryParseMethod<T> parsingMethod)
        {
            _parsingMethod = parsingMethod;
        }

        public bool TryParse(string input, out object value)
        {
            bool success = _parsingMethod(input, out var parsedOutput);
            value = parsedOutput;

            return success;
        }
    }
}