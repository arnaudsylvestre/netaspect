using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.Constructor
{
   public class AfterCallConstructorFieldInOtherTypeParameterWithRealTypeTest :
      NetAspectTest<AfterCallConstructorFieldInOtherTypeParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(null, MyAspect.Constructor);
            var classToWeave_L = new ClassToWeave();
            Assert.AreEqual(2, classToWeave_L.Weaved().Value);
            Assert.AreEqual(".ctor", MyAspect.Constructor.Name);
         };
      }

      public class ClassCalled
      {
          public int Value = 2;

         [MyAspect]
         public ClassCalled()
         {
             
         }
      }

      public class ClassToWeave
      {
         private readonly ClassCalled called;

         public ClassCalled Weaved()
         {
            var classCalled_L = new ClassCalled();
            return classCalled_L;
         }
      }

      public class MyAspect : Attribute
      {
          public static MethodBase Constructor;
         public bool NetAspectAttribute = true;

         public void AfterCallConstructor(ConstructorInfo constructor)
         {
             Constructor = constructor;
         }
      }
   }
}
