using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.Selectors.Members
{
    public class WeavePrivateMethodWithSelectorTest : NetAspectTest<WeavePrivateMethodWithSelectorTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.Instance);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.WeavedPublic();
            Assert.AreEqual(classToWeave_L, MyAspect.Instance);
         };
      }

      public class ClassToWeave
      {
         private void Weaved()
         {
         }

          public void WeavedPublic()
          {
              Weaved();
          }
      }

      public class MyAspect : Attribute
      {
         public static ClassToWeave Instance;
         public bool NetAspectAttribute = true;

         public void BeforeMethod(ClassToWeave instance)
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
