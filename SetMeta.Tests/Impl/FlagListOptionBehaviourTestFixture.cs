﻿using System.Collections.Generic;
using NUnit.Framework;
using SetMeta.Behaviours;
using SetMeta.Entities;
using SetMeta.Impl;

namespace SetMeta.Tests.Impl
{
    [TestFixture]
    public class FlagListOptionBehaviourTestFixture
        : AutoFixtureBase
    {
        [Test]
        public void FlagListOptionBehaviour_WhenWePassNull_ThrowException()
        {
            void Delegate()
            {
                new FlagListOptionBehaviour(null, Fake<List<ListItem>>());
            }

            AssertEx.ThrowsArgumentNullException(Delegate, "optionValue");
        }

        [Test]
        public void FlagListOptionBehaviour_WhenWePassValidItems_TheyAssignedCorrectly()
        {
            var optionValueFactory = new OptionValueFactory();
            var optionValue = optionValueFactory.Create(OptionValueType.String);
            var list = Fake<List<ListItem>>();

            var actual = new FlagListOptionBehaviour(optionValue, list);

            Assert.That(actual.ListItems, Is.EqualTo(list));
        }
    }
}