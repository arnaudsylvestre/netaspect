using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.CallerParameterName
{
   public class AfterCallMethodCallerParameterNameReferencedParameterWithRealTypeReferencedTest :
      NetAspectTest<AfterCallMethodCallerParameterNameReferencedParameterWithRealTypeReferencedTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.ParameterName);
            var classToWeave_L = new ClassToWeave();
            int val = 12;
            classToWeave_L.Weaved(ref val);
            Assert.AreEqual(25, val);
            Assert.AreEqual(12, MyAspect.ParameterName);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public string Method()
         {
            return "Hello";
         }

         public string Weaved(ref int param1)
         {
            return Method();
         }
      }

      public class MyAspect : Attribute
      {
         public static int ParameterName;
         public bool NetAspectAttribute = true;

         public void AfterCallMethod(ref int callerParam1)
         {
            ParameterName = callerParam1;
            callerParam1 = 25;
         }
      }
   }
}
