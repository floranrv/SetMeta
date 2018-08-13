using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using AutoFixture;
using AutoFixture.Kernel;
using FluentAssertions;
using NUnit.Framework;
using SetMeta.Abstract;
using SetMeta.Entities;
using SetMeta.Impl;
using SetMeta.Tests.Util;
using XsdIterator;

namespace SetMeta.Tests.Impl
{
    [TestFixture]
    internal class OptionSetParserV1TestFixture
        : SutBase<OptionSetParserV1, OptionSetParser>
    {
        private static readonly Lazy<IOptionInformator> OptionInformator;

        static OptionSetParserV1TestFixture()
        {
            OptionInformator = new Lazy<IOptionInformator>(() =>
            {
                using (var reader = new XmlTextReader(StaticResources.GetStream("OptionSetV1.xsd")))
                {
                    var xmlSchema = XmlSchema.Read(reader, null);
                    return TraverseXmlSchema(xmlSchema);
                }
            });
        }

        [Test]
        public void Parse_WhenNullXmlTextReaderIsPassed_Throws()
        {
            void Delegate()
            {
                Sut.Parse((XmlTextReader)null);
            }

            AssertEx.ThrowsArgumentNullException(Delegate, "reader");
        }

        [Test]
        public void Parse_WhenXmlHasNoBody_Throws()
        {
            Assert.Throws<XmlException>(() =>
            {
                using (var reader = CreateReader("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>"))
                {
                    Sut.Parse(reader);
                }
            });
        }

        [Test]
        public void Parse_WithOnlyRequaredAttributes_ReturnNotNull()
        {
            var document = GenerateDocumentWithOneOption(a => a.Use == XmlSchemaUse.Required);
            
            var actual = Sut.Parse(CreateReader(document));
            
            Assert.That(actual.Options, Is.Not.Null);
            Assert.That(actual.Version, Is.EqualTo("1"));

            var expected = GetExpectedOptionSet(actual.Options[0]);

            actual.Should().BeEquivalentTo(expected);
        }

        [Test, TestCaseSource(typeof(OptionSetParserV1TestFixture), nameof(OptionKeyValuePairs))]
        public void Parse_WithOnlyRequaredAttributes_ReturnNotNul1l(string attributeName, object attributeValue)
        {
            var document = GenerateDocumentWithOneOption(a => a.Use == XmlSchemaUse.Required || a.Name == attributeName, attributeName, attributeValue);

            var actual = Sut.Parse(CreateReader(document));

            Assert.That(actual.Options, Is.Not.Null);
            Assert.That(actual.Version, Is.EqualTo("1"));

            var expected = GetExpectedOptionSet(actual.Options[0]);

            actual.Should().BeEquivalentTo(expected);
        }

        public static IEnumerable OptionKeyValuePairs
        {
            get
            {
                var fixture = new Fixture();

                yield return new TestCaseData(OptionAttributeKeys.DisplayName, GetNextValue(fixture, OptionInformator.Value.OptionAttributes.First(o => o.Name == OptionAttributeKeys.DisplayName)));
                yield return new TestCaseData(OptionAttributeKeys.DefaultValue, GetNextValue(fixture, OptionInformator.Value.OptionAttributes.First(o => o.Name == OptionAttributeKeys.DefaultValue)));
            }
        }

        private OptionSet GetExpectedOptionSet(Option actual)
        {
            var optionSet = new OptionSet();
            optionSet.Version = "1";

            optionSet.Options.Add(new Option
                {
                    Name = actual.Name,
                    DisplayName = OptionAttributeDefaults.DisplayName,
                    Description = OptionAttributeDefaults.Description,
                    DefaultValue = OptionAttributeDefaults.DefaultValue,
                    ValueType = OptionAttributeDefaults.ValueType
                });

            return optionSet;
        }

        private XmlTextReader CreateReader(string data)
        {
            return new XmlTextReader(new StringReader(data));
        }

        private XmlTextReader CreateReader(XDocument document)
        {
            var stream = new MemoryStream();
            document.Save(stream);
            stream.Position = 0;
            return new XmlTextReader(stream);
        }

        private XDocument GenerateDocumentWithOneOption(Predicate<XmlSchemaAttribute> expectedAttribute, string name = null, object value = null)
        {
            return GenerateDocument(GenerateOptionFunc(expectedAttribute, name, value));
        }

        private Func<IEnumerable<XElement>> GenerateOptionFunc(Predicate<XmlSchemaAttribute> expectedAttribute, string name = null, object value = null)
        {
            return () => new[] { GenerateOption(expectedAttribute, name, value) };
        }

        private XElement GenerateOption(Predicate<XmlSchemaAttribute> expectedAttribute, string name = null, object value = null)
        {
            var option = new XElement(Keys.Option);

            foreach (var optionAttribute in OptionInformator.Value.OptionAttributes.Where(o => expectedAttribute(o)))
            {
                AddAttribute(option,
                    optionAttribute,
                    name == null || name != optionAttribute.Name
                        ? GetNextValue(optionAttribute)
                        : value);
            }

            return option;
        }

        private XDocument GenerateDocument(Func<IEnumerable<XElement>> optionsFunc)
        {
            var declaration = new XDeclaration("1.0", "utf-8", "yes");
            

            var body = new XElement(Keys.OptionSet, optionsFunc());

            return new XDocument(declaration, body);
        }

        private static object GetNextValue(IFixture fixture, XmlSchemaAttribute optionAttribute)
        {
            var type = optionAttribute.AttributeSchemaType.Datatype.ValueType;
            var specimen = new SpecimenContext(fixture).Resolve(type);

            return specimen;
        }

        private object GetNextValue(XmlSchemaAttribute optionAttribute)
        {
            return GetNextValue(AutoFixture, optionAttribute);
        }

        private void AddAttribute(XElement option, XmlSchemaAttribute optionAttribute, object optionValue)
        {
            option.Add(new XAttribute(optionAttribute.Name, Convert.ToString(optionValue)));
        }

        private static IOptionInformator TraverseXmlSchema(XmlSchema xmlSchema)
        {
            var schemaSet = new XmlSchemaSet();
            schemaSet.Add(xmlSchema);
            schemaSet.Compile();

            var visitor = new OptionSetV1XmlSchemaProcessor();
            var iterator = new DefaultXmlSchemaIterator(schemaSet, visitor);

            var enumerator = schemaSet.GlobalElements.Values.GetEnumerator();
            enumerator.MoveNext();
            var globalElement = enumerator.Current;
            globalElement.Accept(iterator);

            return visitor;
        }
    }
}