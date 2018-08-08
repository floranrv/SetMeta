using System;
using System.ComponentModel;
using NUnit.Framework;
using SetMeta.Abstract;
using SetMeta.Impl;

namespace SetMeta.Tests.Impl
{
    [TestFixture]
    internal class OptionValueConverterTestFixture
        : SutBase<OptionValueConverter<string>, IOptionValueConverter<string>>
    {
        [Test]
        public void GetValue_WhenWePassString_ThereShouldBeNoException()
        {
            Assert.DoesNotThrow(() =>
            {
                var actual = Sut.GetValue(Fake<string>());

                Assert.IsNotNull(actual);
            });
        }

        [Test]
        public void GetStringValue_WhenWePassString_ThereShouldBeNoException()
        {
            Assert.DoesNotThrow(() =>
            {
                var actual = Sut.GetStringValue(Fake<string>());

                Assert.IsNotNull(actual);
            });
        }

        [Test]
        public void GetStringValue_WhenFormatProviderIsNull_ThrowException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                Sut.GetStringValue(Fake<string>(), null);
            });

            Assert.That(ex.ParamName, Is.EqualTo("formatProvider"));
        }
    }
}