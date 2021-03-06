using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.CalledParameterName
{
   public class BeforeCallMethodCalledParameterNameParameterWithRealTypeTest :
      NetAspectTest<BeforeCallMethodCalledParameterNameParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.ParameterName);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved();
            Assert.AreEqual(12, MyAspect.ParameterName);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public string Method(int param1)
         {
            return "Hello";
         }

         public string Weaved()
         {
            return Method(12);
         }
      }

      public class MyAspect : Attribute
      {
         public static int ParameterName;
         public bool NetAspectAttribute = true;

         public void BeforeCallMethod(int param1)
         {
            ParameterName = param1;
         }
      }
   }
}
