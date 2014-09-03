using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Return
{
   public class CheckWeaveWithReturnClassTest : NetAspectTest<CheckWeaveWithReturnClassTest.ClassToWeave>
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
         [MyAspect]
         public ClassToWeave Weaved(ClassToWeave toWeave)
         {
            return toWeave;
         }
      }

      public class MyAspect : Attribute
      {
         public static MethodBase Method;
         public bool NetAspectAttribute = true;

         public void Before(MethodBase method)
         {
            Method = method;
         }
      }
   }
}
