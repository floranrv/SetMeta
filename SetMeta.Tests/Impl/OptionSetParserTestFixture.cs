using System;
using System.IO;
using System.Xml;
using NUnit.Framework;
using SetMeta.Abstract;

namespace SetMeta.Tests.Impl
{
    [TestFixture]
    public class OptionSetParserTestFixture 
        : AutoFixtureBase
    {
        [Test]
        public void Create_WhenWePassStringNull_ThrowException()        
        {
            void Delegate()
            {
                OptionSetParser.Create((string)null);
            }

            AssertEx.ThrowsArgumentNullException(Delegate, "version");
        }

        [Test]
        public void Create_WhenWePassEmptyString_ThrowException()
        {
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                OptionSetParser.Create("");
            });

            Assert.That(ex.Message, Is.EqualTo($"Can't create '{nameof(OptionSetParser)}' of given version ''."));
        }

        [Test]
        public void Create_WhenWePassStreamNull_ThrowException()
        {
            void Delegate()
            {
                OptionSetParser.Create((Stream)null);
            }

            AssertEx.ThrowsArgumentNullException(Delegate, "stream");
        }

        [Test]
        public void Create_WhenWePassReaderNull_ThrowException()
        {
            void Delegate()
            {
                OptionSetParser.Create((XmlTextReader)null);
            }

            AssertEx.ThrowsArgumentNullException(Delegate, "reader");
        }
    }
}
