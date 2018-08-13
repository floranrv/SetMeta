namespace SetMeta.Entities
{
    public class Option
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public object DefaultValue { get; set; }
        public OptionValueType ValueType { get; set; }
    }

    internal static class Keys
    {
        public static string OptionSet = "optionSet";
        public static string Option = "option";
    }

    internal static class OptionSetAttributeKeys
    {
        public static string Version = "version";
    }

    internal static class OptionAttributeKeys
    {
        public static string Name = "name";
        public static string DisplayName = "displayName";
        public static string Description = "description";
        public static string DefaultValue = "defaultValue";
        public static string ValueType = "valueType";
    }

    internal static class OptionAttributeDefaults
    {
        public static string DisplayName = null;
        public static string Description = null;
        public static object DefaultValue = null;
        public static OptionValueType ValueType = OptionValueType.String;
    }
}