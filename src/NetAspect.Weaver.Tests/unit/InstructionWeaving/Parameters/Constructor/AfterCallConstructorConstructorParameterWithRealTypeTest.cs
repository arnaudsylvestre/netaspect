using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.Constructor
{
   public class AfterCallConstructorConstructorParameterWithRealTypeTest :
      NetAspectTest<AfterCallConstructorConstructorParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(null, MyAspect.Constructor);
            var classToWeave_L = ClassToWeave.Create();
            Assert.AreEqual("ClassToWeave", MyAspect.Constructor.DeclaringType.Name);
         };
      }

      public class ClassToWeave
      {


         [MyAspect]
         public ClassToWeave()
         {

         }

         public static ClassToWeave Create()
         {
            return new ClassToWeave();
         }
      }

      public class MyAspect : Attribute
      {
         public static MethodBase Constructor;
         public bool NetAspectAttribute = true;

         public void AfterCallConstructor(MethodBase constructor)
         {
            Constructor = constructor;
         }
      }
   }
}
