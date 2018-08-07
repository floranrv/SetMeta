using System;
using System.IO;
using System.Xml;
using SetMeta.Entities;

namespace SetMeta.Abstract
{
    public abstract class OptionSetParser
    {
        public abstract Version Version { get; }

        public OptionSetParser Create(Version version)
        {
            throw new NotImplementedException();
        }

        public OptionSetParser Create(Stream stream)
        {
            throw new NotImplementedException();
        }

        public OptionSetParser Create(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public abstract OptionSet Parse(XmlReader reader);

        public abstract OptionSet Parse(Stream stream);

        public abstract OptionSet Parse(string data);
    }
}