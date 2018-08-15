using System.ComponentModel;
using SetMeta.Abstract;

namespace SetMeta.Impl
{
    public class OptionValueFactory
        : IOptionValueFactory
    {
        public IOptionValue Create(OptionValueType optionValueType)
        {
            switch (optionValueType)
            {
                case OptionValueType.String:
                    return new OptionValue<string>(optionValueType, new OptionValueConverter<string>());
                default:
                    throw new InvalidEnumArgumentException(nameof(optionValueType), (int)optionValueType, typeof(OptionValueType));
            }
        }
    }
}