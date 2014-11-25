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
            ClassToWeave res = classToWeave_L.Weaved(classToWeave_L, "param1");
            Assert.AreEqual("Weaved", MyAspect.Method.Name);
            Assert.AreEqual(classToWeave_L, res);
         };
      }

      public class ClassToWeave
      {
          [MyAspect]
          public T Weaved<T>(T toWeave, string param1)
          {
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
