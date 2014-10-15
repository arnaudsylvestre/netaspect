using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.Instance
{
   public class AfterMethodInstanceParameterWithRealTypeTest :
      NetAspectTest<AfterMethodInstanceParameterWithRealTypeTest.ClassToWeave>
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
         [MyAspect]
         public void Weaved()
         {
         }
      }

      public class MyAspect : Attribute
      {
         public static ClassToWeave Instance;
         public bool NetAspectAttribute = true;

         public void AfterMethod(ClassToWeave instance)
         {
            Instance = instance;
         }
      }
   }
}