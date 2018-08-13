using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using Moq;

namespace SetMeta.Tests.Util
{
    public class StrictAutoMoqCustomization : ICustomization
    {
        public StrictAutoMoqCustomization() : this(new MockRelay()) { }

        public StrictAutoMoqCustomization(ISpecimenBuilder relay)
        {
            Relay = relay ?? throw new ArgumentNullException(nameof(relay));
        }

        public ISpecimenBuilder Relay { get; }

        public void Customize(IFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            fixture.Customizations.Add(new MockPostprocessor(new MethodInvoker(new StrictMockConstructorQuery())));
            fixture.ResidueCollectors.Add(Relay);
        }
    }

    public class StrictMockConstructorMethod : IMethod
    {
        private readonly ConstructorInfo _ctor;
        private readonly ParameterInfo[] _paramInfos;

        public StrictMockConstructorMethod(ConstructorInfo ctor, ParameterInfo[] paramInfos)
        {
            this._ctor = ctor ?? throw new ArgumentNullException(nameof(ctor));
            this._paramInfos = paramInfos;
        }

        public IEnumerable<ParameterInfo> Parameters => _paramInfos;

        public object Invoke(IEnumerable<object> parameters) => _ctor.Invoke(parameters?.ToArray() ?? new object[] { });
    }

    public class StrictMockConstructorQuery : IMethodQuery
    {
        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            if (!IsMock(type))
            {
                return Enumerable.Empty<IMethod>();
            }

            if (!GetMockedType(type).IsInterface && !GetMockedType(type).IsAbstract && !IsDelegate(type))
            {
                return Enumerable.Empty<IMethod>();
            }

            var ctor = type.GetConstructor(new[] { typeof(MockBehavior) });

            return new IMethod[]
                {
                    new StrictMockConstructorMethod(ctor, ctor.GetParameters())
                };
        }

        private static bool IsMock(Type type)
        {
            return type != null && type.IsGenericType && typeof(Mock<>).IsAssignableFrom(type.GetGenericTypeDefinition()) && !GetMockedType(type).IsGenericParameter;
        }

        private static Type GetMockedType(Type type)
        {
            return type.GetGenericArguments().Single();
        }

        internal static bool IsDelegate(Type type)
        {
            return typeof(MulticastDelegate).IsAssignableFrom(type.BaseType);
        }
    }
}