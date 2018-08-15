namespace SetMeta.Abstract
{
    public interface IOptionValueFactory
    {
        IOptionValue Create(OptionValueType optionValueType);
    }
}