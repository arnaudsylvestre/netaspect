using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.ParameterWeaving.Constructor.OnException
{
    [TestFixture]
    public class OnExceptionConstructorParameterValueTest
    {
       [Test]
       public void CheckReferenced()
       {
          throw new NotImplementedException();
       }


       [Test]
        public void CheckInstanceWithRealType()
        {
            throw new NotImplementedException();
        }
   }
}