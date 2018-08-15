using SetMeta.Abstract;

namespace SetMeta.Behaviours
{
    public class RangedOptionBehaviour
        : OptionBehaviour
    {
        public RangedOptionBehaviour(IOptionValue optionValue, string minValue, string maxValue)
            : base(optionValue)
        {
            IsMinValueExists = true;
            MinValue = minValue;
            IsMaxValueExists = true;
            MaxValue = maxValue;
        }

        public RangedOptionBehaviour(IOptionValue optionValue, string value, bool isMin)
            : base(optionValue)
        {
            if (isMin)
            {
                IsMinValueExists = true;
                MinValue = value;
                IsMaxValueExists = false;
                MaxValue = null;
            }
            else
            {
                IsMinValueExists = false;
                MinValue = null;
                IsMaxValueExists = true;
                MaxValue = value;
            }
        }

        public string MinValue { get; private set; }
        public bool IsMinValueExists { get; private set; }
        public string MaxValue { get; private set; }
        public bool IsMaxValueExists { get; private set; }
    }
}