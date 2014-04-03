using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.ParameterWeaving.Constructor.OnException
{
    [TestFixture]
    public class OnExceptionConstructorParameterTest
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