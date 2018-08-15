using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using AutoFixture;
using AutoFixture.Kernel;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using SetMeta.Abstract;
using SetMeta.Behaviours;
using SetMeta.Entities;
using SetMeta.Impl;
using SetMeta.Tests.Util;
using SetMeta.Util;
using XsdIterator;
using Assert = NUnit.Framework.Assert;

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
        public void OptionSetParserV1_WhenWePassNull_ThrowException()
        {
            void Delegate()
            {
                new OptionSetParserV1(null);
            }

            AssertEx.ThrowsArgumentNullException(Delegate, "optionValueFactory");
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

        [TestCase(OptionAttributeKeys.Name, typeof(string), nameof(Option.Name))]
        [TestCase(OptionAttributeKeys.DefaultValue, typeof(string), nameof(Option.DefaultValue))]
        [TestCase(OptionAttributeKeys.Description, typeof(string), nameof(Option.Description))]
        [TestCase(OptionAttributeKeys.DisplayName, typeof(string), nameof(Option.DisplayName))]
        [TestCase(OptionAttributeKeys.ValueType, typeof(OptionValueType), nameof(Option.ValueType))]
        [TestCategory("Option")]
        public void Parse_WhenItPresentInOption_ShouldReadAttribute(string attributeName, Type attributeValueType, string propertyName)
        {
            DataConversion.AddParser<object>((delegate(string input, out object value)
            {
                value = input;
                return true;
            }));

            var attributeValue = GetNextValue(attributeValueType);

            var document = GenerateDocumentWithOneOption(a => a.Use == XmlSchemaUse.Required || a.Name == attributeName, attributeName, attributeValue);

            var actual = Sut.Parse(CreateReader(document));

            Assert.That(actual.Options, Is.Not.Null);
            Assert.That(actual.Version, Is.EqualTo("1"));          

            PropertyInfo propertyInfo = typeof(Option).GetProperty(propertyName);
            Assert.That(propertyInfo.GetValue(actual.Options[0]), Is.EqualTo(attributeValue));
        }

        [TestCase(OptionAttributeKeys.DefaultValue, typeof(string), nameof(Option.DefaultValue), OptionAttributeDefaults.DefaultValue)]
        [TestCase(OptionAttributeKeys.Description, typeof(string), nameof(Option.Description), OptionAttributeDefaults.Description)]
        [TestCase(OptionAttributeKeys.DisplayName, typeof(string), nameof(Option.DisplayName), OptionAttributeDefaults.DisplayName)]
        [TestCase(OptionAttributeKeys.ValueType, typeof(OptionValueType), nameof(Option.ValueType), OptionAttributeDefaults.ValueType)]
        [TestCategory("Option")]
        public void Parse_WhenItAbsentInOption_ShouldReturnDefaultValue(string attributeName, Type attributeValueType, string propertyName, object attributeValue)
        {
            DataConversion.AddParser<object>((delegate (string input, out object value)
            {
                value = input;
                return true;
            }));

            var document = GenerateDocumentWithOneOption(a => a.Use == XmlSchemaUse.Required);

            var actual = Sut.Parse(CreateReader(document));

            Assert.That(actual.Options, Is.Not.Null);
            Assert.That(actual.Version, Is.EqualTo("1"));

            PropertyInfo propertyInfo = typeof(Option).GetProperty(propertyName);
            Assert.That(propertyInfo.GetValue(actual.Options[0]), Is.EqualTo(attributeValue));
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

            var optionValueFactory = new OptionValueFactory();
            var optionValue = optionValueFactory.Create(OptionValueType.String);

            optionSet.Options.Add(new Option
                {
                    Name = actual.Name,
                    DisplayName = OptionAttributeDefaults.DisplayName,
                    Description = OptionAttributeDefaults.Description,
                    DefaultValue = OptionAttributeDefaults.DefaultValue,
                    ValueType = OptionAttributeDefaults.ValueType,
                    Behaviour = new SimpleOptionBehaviour(optionValue)
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

        private object GetNextValue(Type type)
        {
            var specimen = new SpecimenContext(AutoFixture).Resolve(type);

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