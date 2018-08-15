using SetMeta.Abstract;
using SetMeta.Impl;

namespace SetMeta.Behaviours
{
    public class SimpleOptionBehaviour
        : OptionBehaviour
    {
        public SimpleOptionBehaviour(IOptionValue optionValue)
            : base(optionValue)
        {
        }       
    }
}