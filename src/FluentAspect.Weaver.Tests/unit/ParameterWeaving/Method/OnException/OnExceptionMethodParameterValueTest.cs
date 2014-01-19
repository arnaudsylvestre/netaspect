using System;
using FluentAspect.Weaver.Tests.Core;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.ParameterWeaving.Method.OnException
{
    [TestFixture]
   public class OnExceptionMethodParameterValueTest
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