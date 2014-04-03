using System;
using NUnit.Framework;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit
{

    [TestFixture]
    public abstract class NetAspectTest<T, U>
    {
        protected virtual Action CreateEnsure()
        {
            return () => { };
        }

        protected virtual Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return e => { };
        }

        [Test]
        public void DoTest()
        {
            RunWeavingTest.For<T, U>(CreateErrorHandlerProvider(), CreateEnsure());
        }
    }

    [TestFixture]
    public abstract class NetAspectTest<T> : NetAspectTest<T, T>
    {
        
    }
}