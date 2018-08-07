using System.ComponentModel;
using NUnit.Framework;
using SetMeta.Abstract;
using SetMeta.Impl;

namespace SetMeta.Tests.Impl
{
    [TestFixture]
    public class OptionValueFactoryTestFixture
        : SutBase<OptionValueFactory, IOptionValueFactory>
    {
        [Test]
        public void Create_WhenWePassAnyOptionValueType_ThereShouldBeNoException([Values]OptionValueType optionValueType)
        {
            Assert.DoesNotThrow(() =>
            {
                var actual = Sut.Create(optionValueType, Fake<string>());

                Assert.IsNotNull(actual);
            });
        }

        [TestCase("")]
        [TestCase(null)]
        public void Create_WhenWePassEmptyOrNullString_ThereShouldBeNoException(string value)
        {
            Assert.DoesNotThrow(() =>
            {
                var actual = Sut.Create(Fake<OptionValueType>(), value);

                Assert.IsNotNull(actual);
            });
        }

        [Test]
        public void Create_WhenOptionValueTypeIsUnrecognized_ThrowException()
        {
            var ex = Assert.Throws<InvalidEnumArgumentException>(() =>
            {
                Sut.Create((OptionValueType)255, Fake<string>());            
            });

            Assert.That(ex.Message, Does.Contain("optionValueType"));
            Assert.That(ex.ParamName, Is.EqualTo("optionValueType"));
        }
    }
}