using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Constructors.Parameters.After.Caller
{
   public class AfterCallConstructorCallerParameterWithRealTypeTest :
      NetAspectTest<AfterCallConstructorCallerParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.Caller);
            var classToWeave = new ClassToWeave();
            ClassToWeave classToWeave_L = classToWeave.Create();
            Assert.AreEqual(classToWeave, MyAspect.Caller);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public ClassToWeave()
         {
         }

         public ClassToWeave Create()
         {
            return new ClassToWeave();
         }
      }

      public class MyAspect : Attribute
      {
         public static ClassToWeave Caller;
         public bool NetAspectAttribute = true;

         public void AfterCallConstructor(ClassToWeave caller)
         {
            Caller = caller;
         }
      }
   }
}
