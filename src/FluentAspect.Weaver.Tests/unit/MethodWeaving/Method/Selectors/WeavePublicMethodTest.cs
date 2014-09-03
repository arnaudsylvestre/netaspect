using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Attributes.Visibility
{
   public class WeavePublicMethodWithSelectorTest : NetAspectTest<WeavePublicMethodWithSelectorTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.Instance);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved();
            Assert.AreEqual(classToWeave_L, MyAspect.Instance);
         };
      }

      public class ClassToWeave
      {
         public void Weaved()
         {
         }
      }

      public class MyAspect : Attribute
      {
         public static ClassToWeave Instance;
         public bool NetAspectAttribute = true;

         public void Before(ClassToWeave instance)
         {
            Instance = instance;
         }


         public static bool SelectMethod(MethodInfo method)
         {
            return method.Name == "Weaved";
         }
      }
   }
}
