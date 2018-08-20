using System.Collections.Generic;
using SetMeta.Abstract;
using SetMeta.Entities;

namespace SetMeta.Behaviours
{
    public class MultiListOptionBehaviour
        : OptionBehaviour
    {
        internal MultiListOptionBehaviour(IOptionValue optionValue, IEnumerable<ListItem> validItems, bool sorted = false, string separator = ";")
            : base(optionValue)
        {
            ListItems = new List<ListItem>(validItems);
            Sorted = sorted;
            Separator = separator;
        }

        public List<ListItem> ListItems { get; private set; }
        public bool Sorted { get; private set; }
        public string Separator { get; private set; }

    }
}