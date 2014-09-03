using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Attributes.Static
{
   public class WeaveStaticMethodTest : NetAspectTest<WeaveStaticMethodTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.Method);
            ClassToWeave.Weaved();
            Assert.AreEqual("Weaved", MyAspect.Method.Name);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public static void Weaved()
         {
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
