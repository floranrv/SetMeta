using System.Xml;
using SetMeta.Abstract;
using SetMeta.Entities;

namespace SetMeta.Impl
{
    public class OptionSetParserV1
        : OptionSetParser
    {
        public override string Version => "1";

        public override OptionSet Parse(XmlTextReader reader)
        {
            throw new System.NotImplementedException();
        }
    }
}