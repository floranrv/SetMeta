using System;
using SetMeta.Impl;

namespace SetMeta.Abstract
{
    public abstract class OptionBehaviour
    {
        protected OptionBehaviour(IOptionValue optionValue)
        {
            OptionValue = optionValue ?? throw new ArgumentNullException(nameof(optionValue));
        }

        /// <summary>
        /// Тип значения настройки
        /// </summary>
        public OptionValueType OptionValueType => OptionValue.OptionValueType;

        /// <summary>
        /// Объект со значением настройи
        /// </summary>
        protected IOptionValue OptionValue { get; private set; }

        /// <summary>
        /// Получает значение настройки из строки
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object Value(string value)
        {
            return OptionValue.GetValue(value);
        }

        /// <summary>
        /// Получает строку из значения настройки
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string StringValue(object value)
        {
            return OptionValue.GetStringValue(value);
        }
    }
}
