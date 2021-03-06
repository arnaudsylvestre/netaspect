using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.CallerParameters
{
   public class BeforeCallMethodCallerParametersParameterWithRealTypeTest :
      NetAspectTest<BeforeCallMethodCallerParametersParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.CallerParameters);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved(1, 2);
            Assert.AreEqual(
               new object[]
               {
                  1, 2
               },
               MyAspect.CallerParameters);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public string Method()
         {
            return "Hello";
         }

         public string Weaved(int param1, int param2)
         {
            return Method();
         }
      }

      public class MyAspect : Attribute
      {
         public static object[] CallerParameters;
         public bool NetAspectAttribute = true;

         public void BeforeCallMethod(object[] callerParameters)
         {
            CallerParameters = callerParameters;
         }
      }
   }
}
