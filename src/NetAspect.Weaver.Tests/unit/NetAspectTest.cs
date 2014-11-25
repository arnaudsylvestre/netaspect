using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Model.Errors;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit
{
    using System.Reflection;

    public class ClassToWeaveCheckMethod
    {
        [Test]
        public void Check()
        {
            Weaved("string", "string");
        }

        //[MyAspect]
        public T Weaved<T>(T toWeave, string param1)
        {
            MethodInfo myMethod = null;
            var methods = GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var methodInfo in methods)
            {
                if (methodInfo.Name != "Weaved")
                    continue;
                if (methodInfo.GetGenericArguments().Length != 1)
                    continue;
                var parameters = methodInfo.GetParameters();
                if (parameters.Length != 2)
                    continue;
                if (parameters[0].ParameterType.Name != "T")
                    continue;
                if (parameters[1].ParameterType.FullName != "System.String")
                    continue;
                myMethod = methodInfo;
                break;
            }

            Assert.NotNull(myMethod, "Elle est nulle !!!");
            return toWeave;
        }

        public T Weaved<T, T1>(T toWeave)
        {
            return toWeave;
        }

        public T Weaved<T>(T toWeave, int param)
        {
            return toWeave;
        }
    }

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
