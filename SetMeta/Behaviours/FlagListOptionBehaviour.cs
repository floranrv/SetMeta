﻿using System.Collections.Generic;
using SetMeta.Abstract;
using SetMeta.Entities;
using SetMeta.Impl;

namespace SetMeta.Behaviours
{
    public class FlagListOptionBehaviour
        : OptionBehaviour
    {
        internal FlagListOptionBehaviour(IOptionValue optionValue, IEnumerable<ListItem> validItems)
            : base(optionValue)
        {
            ListItems = new List<ListItem>(validItems);
        }

        public List<ListItem> ListItems { get; private set; }
    }
}