using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Constructors.Parameters.After.CalledParameterName
{
   public class AfterCallConstructorCalledParameterNameParameterWithRealTypeTest :
      NetAspectTest<AfterCallConstructorCalledParameterNameParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.ParameterName);
            ClassToWeave classToWeave_L = ClassToWeave.Create();
            Assert.AreEqual(12, MyAspect.ParameterName);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public ClassToWeave(int param1)
         {
         }

         public static ClassToWeave Create()
         {
            return new ClassToWeave(12);
         }
      }

      public class MyAspect : Attribute
      {
         public static int ParameterName;
         public bool NetAspectAttribute = true;

         public void AfterCallConstructor(int calledParam1)
         {
            ParameterName = calledParam1;
         }
      }
   }
}
