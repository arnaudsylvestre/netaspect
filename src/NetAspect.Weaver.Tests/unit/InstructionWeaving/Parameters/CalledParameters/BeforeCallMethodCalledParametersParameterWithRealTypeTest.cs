using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.CalledParameters
{
   public class BeforeCallMethodCalledParametersParameterWithRealTypeTest :
      NetAspectTest<BeforeCallMethodCalledParametersParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.Parameters);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved();
            Assert.AreEqual(
               new object[]
               {
                  1, 2
               },
               MyAspect.Parameters);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public string Method(int param1, int param2)
         {
            return "Hello";
         }

         public string Weaved()
         {
            return Method(1, 2);
         }
      }

      public class MyAspect : Attribute
      {
         public static object[] Parameters;
         public bool NetAspectAttribute = true;

         public void BeforeCallMethod(object[] parameters)
         {
            Parameters = parameters;
         }
      }
   }
}
