using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.Before.Instance
{
   public class BeforeConstructorInstanceParameterWithRealTypeTest :
      NetAspectTest<BeforeConstructorInstanceParameterWithRealTypeTest.ClassToWeave>
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
         [MyAspect]
         public ClassToWeave()
         {
         }
      }

      public class MyAspect : Attribute
      {
         public static ClassToWeave Instance;
         public bool NetAspectAttribute = true;

         public void BeforeConstructor(ClassToWeave instance)
         {
            Instance = instance;
         }
      }
   }
}
