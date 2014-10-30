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
         return () =>
         {
            Assert.IsNull(MyAspect.Method);
            var classToWeave_L = new ClassToWeave();
            ClassToWeave res = classToWeave_L.Weaved(classToWeave_L);
            Assert.AreEqual("Weaved", MyAspect.Method.Name);
            Assert.AreEqual(classToWeave_L, res);
         };
      }

      public class ClassToWeave
      {
         //[MyAspect]
         public T Weaved<T>(T toWeave)
         {
             var method = GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                            .Where(s => s.Name == "Weaved" && s.GetParameters()[0].ParameterType == typeof(T));

             Assert.NotNull(method, "Elle est nulle !!!");
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
