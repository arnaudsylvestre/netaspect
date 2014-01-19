using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Methods.Parameters.OnException
{
    [TestFixture]
    public class OnExceptionCallMethodExceptionParameterTest
    {
        [Test]
        public void CheckCallMethodWithException()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void CheckCallMethodWithExceptionWithWrongType()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void CheckCallMethodWithExceptionReferenced()
        {
            throw new NotImplementedException();
        }
    }
}