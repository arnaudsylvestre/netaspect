using System;
using FluentAspect.Weaver.Core.Errors;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit
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