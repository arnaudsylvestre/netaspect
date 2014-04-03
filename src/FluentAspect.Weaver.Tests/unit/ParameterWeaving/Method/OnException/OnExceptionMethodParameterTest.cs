using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.ParameterWeaving.Method.OnException
{
    [TestFixture]
    public class OnExceptionMethodParameterTest
    {
        [Test]
        public void CheckInstanceWithRealType()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void CheckReferenced()
        {
            throw new NotImplementedException();
        }
    }
}