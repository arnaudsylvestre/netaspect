using System;
using FluentAspect.Weaver.Tests.acceptance;
using FluentAspect.Weaver.Tests.acceptance.Weaving.Calls.Fields;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.OnException
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