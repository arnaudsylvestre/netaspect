using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Model.Errors;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit
{
    [TestFixture]
   public abstract class NetAspectTest<T, U>
   {
      protected virtual Action CreateEnsure()
      {
         return () => { };
      }

      protected virtual Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
      {
         return e => { };
      }

      public void Check()
      {
         CreateEnsure()();
      }

      [Test]
      public void DoTest()
      {
         RunWeavingTest.For<T, U>(GetType(), CreateErrorHandlerProvider(), CreateEnsure());
      }
   }

   [TestFixture]
   public abstract class NetAspectTest<T> : NetAspectTest<T, T>
   {
      protected NetAspectTest()
      {
      }

      protected NetAspectTest(string beforeMethodWeavingPossibilities_P, string methodweavingbefore_P, string methodweaving_P)
      {
      }
   }
}
