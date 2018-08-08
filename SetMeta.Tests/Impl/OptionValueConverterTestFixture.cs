using System;
using System.ComponentModel;
using System.Globalization;
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
        public void GetValue_WhenWePassNull_NullShouldBeReturned()
        {
            Assert.DoesNotThrow(() =>
            {
                var actual = Sut.GetValue(null);

                Assert.IsNull(actual);
            });
        }

        [Test]
        public void GetValue_WhenWePassEmptyString_EmptyStringShouldBeReturned()
        {
            Assert.DoesNotThrow(() =>
            {
                var actual = Sut.GetValue("");

                Assert.IsEmpty(actual);
            });
        }

        [Test]
        public void GetValue_WhenWePassString_SameStringShouldBeReturned()
        {
            Assert.DoesNotThrow(() =>
            {
                string temp = Fake<string>();

                var actual = Sut.GetValue(temp);

                Assert.That(actual, Is.EqualTo(temp));
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
        public void GetStringValue_WhenWePassNull_NullShouldBeReturned()
        {
            Assert.DoesNotThrow(() =>
            {
                var actual = Sut.GetStringValue(null);

                Assert.IsNull(actual);
            });
        }

        [Test]
        public void GetStringValue_WhenWePassEmptyString_EmptyStringShouldBeReturned()
        {
            Assert.DoesNotThrow(() =>
            {
                var actual = Sut.GetStringValue("");

                Assert.IsEmpty(actual);
            });
        }

        [Test]
        public void GetStringValue_WhenWePassString_SameStringShouldBeReturned()
        {
            Assert.DoesNotThrow(() =>
            {
                string temp = Fake<string>();

                var actual = Sut.GetStringValue(temp);

                Assert.That(actual, Is.EqualTo(temp));
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

        [Test]
        public void GetStringValueRuCulture_WhenWePassNull_NullShouldBeReturned()
        {
            Assert.DoesNotThrow(() =>
            {
                var actual = Sut.GetStringValue(null, new CultureInfo("ru-RU"));

                Assert.IsNull(actual);
            });
        }

        [Test]
        public void GetStringValueRuCulture_WhenWePassEmptyString_EmptyStringShouldBeReturned()
        {
            Assert.DoesNotThrow(() =>
            {
                var actual = Sut.GetStringValue("", new CultureInfo("ru-RU"));

                Assert.IsEmpty(actual);
            });
        }

        [Test]
        public void GetStringValueRuCulture_WhenWePassString_SameStringShouldBeReturned()
        {
            Assert.DoesNotThrow(() =>
            {
                string temp = Fake<string>();

                var actual = Sut.GetStringValue(temp, new CultureInfo("ru-RU"));

                Assert.That(actual, Is.EqualTo(temp));
            });
        }

        [Test]
        public void GetStringValueUsCulture_WhenWePassNull_NullShouldBeReturned()
        {
            Assert.DoesNotThrow(() =>
            {
                var actual = Sut.GetStringValue(null, new CultureInfo("en-US"));

                Assert.IsNull(actual);
            });
        }

        [Test]
        public void GetStringValueUsCulture_WhenWePassEmptyString_EmptyStringShouldBeReturned()
        {
            Assert.DoesNotThrow(() =>
            {
                var actual = Sut.GetStringValue("", new CultureInfo("en-US"));

                Assert.IsEmpty(actual);
            });
        }

        [Test]
        public void GetStringValueUsCulture_WhenWePassString_SameStringShouldBeReturned()
        {
            Assert.DoesNotThrow(() =>
            {
                string temp = Fake<string>();

                var actual = Sut.GetStringValue(temp, new CultureInfo("en-US"));

                Assert.That(actual, Is.EqualTo(temp));
            });
        }
    }
}