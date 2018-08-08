using System;
using System.Xml;
using System.Xml.Linq;
using SetMeta.Abstract;
using SetMeta.Entities;
using SetMeta.Util;

namespace SetMeta.Impl
{
    public class OptionSetParserV1
        : OptionSetParser
    {
        public override string Version => "1";

        public override OptionSet Parse(XmlTextReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            var optionSet = new OptionSet();
            var document = XDocument.Load(reader);
            var body = document.Root;
            if (body == null)
                throw new InvalidOperationException("Xml body is absent.");

            optionSet.Version = Version;

            foreach (var element in body.Elements("option"))
            {
                optionSet.Options.Add(ParseOption(element));
            }

            return optionSet;
        }

        private Option ParseOption(XElement root)
        {
            var option = new Option();

            option.Name = root.GetAttributeValue<string>("name");
            option.DisplayName = root.GetAttributeValue<string>("displayName");
            option.Description = root.TryGetAttributeValue<string>("description", null);
            option.DefaultValue = root.TryGetAttributeValue<string>("defaultValue", null);
            option.ValueType = root.TryGetAttributeValue("valueType", OptionValueType.String);

            return option;
        }
    }
}