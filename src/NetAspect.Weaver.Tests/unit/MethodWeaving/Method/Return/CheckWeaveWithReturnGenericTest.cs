using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Return
{
   public class CheckWeaveWithReturnGenericTest : NetAspectTest<CheckWeaveWithReturnGenericTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
          //HERE
         return () =>
         {
            Assert.IsNull(MyAspect.Method);
            var classToWeave_L = new ClassToWeave();
            ClassToWeave res = classToWeave_L.Weaved(classToWeave_L, "param1");
            Assert.AreEqual("Weaved", MyAspect.Method.Name);
            Assert.AreEqual(classToWeave_L, res);
         };
      }

      public class ClassToWeave
      {
          //[MyAspect]
          public T Weaved<T>(T toWeave, string param1)
          {
              MethodInfo myMethod = null;
             var methods = GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
             foreach (var methodInfo in methods)
             {
                 if (methodInfo.Name != "Weaved")
                     continue;
                 if (methodInfo.GetGenericArguments().Count() != 1)
                     continue;
                 var parameters = methodInfo.GetParameters();
                 if (parameters.Length != 2)
                     continue;
                 var parameterType = parameters[0].ParameterType;   
                 if (parameterType != typeof(T))
                     continue;
                 if (parameters[1].ParameterType != typeof(string))
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

      public class MyAspect : Attribute
      {
         public static MethodBase Method;
         public bool NetAspectAttribute = true;

         public void BeforeMethod(MethodInfo method)
         {
            Method = method;
         }
      }
   }
}
