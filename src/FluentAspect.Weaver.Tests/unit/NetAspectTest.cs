using System;
using System.Collections.Generic;
using NUnit.Framework;
using NetAspect.Weaver.Core.Model.Errors;

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

        [Test]
        public void DoTest()
        {
            RunWeavingTest.For<T, U>(GetType(), CreateErrorHandlerProvider(), CreateEnsure());
        }

       public void Check()
       {
          CreateEnsure()();
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