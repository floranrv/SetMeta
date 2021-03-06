﻿using System.Collections.Generic;
using NUnit.Framework;
using SetMeta.Behaviours;
using SetMeta.Entities;
using SetMeta.Impl;

namespace SetMeta.Tests.Impl
{
    [TestFixture]
    public class MultiListOptionBehaviourTestFixture
        : AutoFixtureBase
    {
        [Test]
        public void MultiListOptionBehaviour_WhenWePassNull_ThrowException()
        {
            void Delegate()
            {
                new MultiListOptionBehaviour(null, Fake<List<ListItem>>());
            }

            AssertEx.ThrowsArgumentNullException(Delegate, "optionValue");
        }

        [Test]
        public void MultiListOptionBehaviour_WhenWePassValidItems_TheyAssignedCorrectly()
        {
            var optionValueFactory = new OptionValueFactory();
            var optionValue = optionValueFactory.Create(OptionValueType.String);
            var list = Fake<List<ListItem>>();

            var actual = new MultiListOptionBehaviour(optionValue, list);

            Assert.That(actual.ListItems, Is.EqualTo(list));
        }

        [TestCase(true, "/")]
        [TestCase(true, ";")]
        [TestCase(false, "/")]
        public void MultiListOptionBehaviour_WhenWePassSortedAndSeparator_TheyAssignedCorrectly(bool sorted, string separator)
        {
            var optionValueFactory = new OptionValueFactory();
            var optionValue = optionValueFactory.Create(OptionValueType.String);
            var list = Fake<List<ListItem>>();

            var actual = new MultiListOptionBehaviour(optionValue, list, sorted, separator);

            Assert.That(actual.Sorted, Is.EqualTo(sorted));
            Assert.That(actual.Separator, Is.EqualTo(separator));
        }
    }
}