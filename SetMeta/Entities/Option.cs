namespace SetMeta.Entities
{
    public class Option
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string DefaultValue { get; set; }
        public OptionValueType ValueType { get; set; }
    }
}