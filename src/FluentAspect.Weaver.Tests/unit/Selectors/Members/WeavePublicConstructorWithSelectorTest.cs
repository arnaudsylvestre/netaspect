using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.Selectors.Members
{
   public class WeavePublicConstructorWithSelectorTest : NetAspectTest<WeavePublicConstructorWithSelectorTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.Instance);
            var classToWeave_L = new ClassToWeave();
            Assert.AreEqual(classToWeave_L, MyAspect.Instance);
         };
      }

      public class ClassToWeave
      {
      }

      public class MyAspect : Attribute
      {
         public static ClassToWeave Instance;
         public bool NetAspectAttribute = true;

         public void BeforeConstructor(ClassToWeave instance)
         {
            Instance = instance;
         }


         public static bool SelectConstructor(ConstructorInfo constructor)
         {
            return constructor.DeclaringType.Name == "ClassToWeave";
         }
      }
   }
}
