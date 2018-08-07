using System;
using System.Collections.Generic;

namespace SetMeta.Entities
{
    public class OptionSet
    {
        public Version Version { get; set; }
        public IList<Option> Options { get; set; }
    }
}