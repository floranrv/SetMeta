using System;
using System.IO;
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
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                OptionSetParser.Create((string) null);
            });

            Assert.That(ex.ParamName, Is.EqualTo("version"));
        }

        [Test]
        public void Create_WhenWePassEmptyString_ThrowException()
        {
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                OptionSetParser.Create("");
            });            
        }

        [Test]
        public void Create_WhenWePassStreamNull_ThrowException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                OptionSetParser.Create((Stream) null);
            });

            Assert.That(ex.ParamName, Is.EqualTo("stream"));
        }
    }
}