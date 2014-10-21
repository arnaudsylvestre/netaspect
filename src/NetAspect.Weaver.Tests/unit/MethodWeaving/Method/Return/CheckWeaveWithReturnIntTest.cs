using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Return
{
   public class CheckWeaveWithReturnIntTest : NetAspectTest<CheckWeaveWithReturnIntTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.Method);
            int res = new ClassToWeave().Weaved();
            Assert.AreEqual("Weaved", MyAspect.Method.Name);
            Assert.AreEqual(12, res);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public int Weaved()
         {
            return 12;
         }
      }

      public class MyAspect : Attribute
      {
         public static MethodBase Method;
         public bool NetAspectAttribute = true;

         public void BeforeMethod(MethodBase method)
         {
            Method = method;
         }
      }
   }
}
