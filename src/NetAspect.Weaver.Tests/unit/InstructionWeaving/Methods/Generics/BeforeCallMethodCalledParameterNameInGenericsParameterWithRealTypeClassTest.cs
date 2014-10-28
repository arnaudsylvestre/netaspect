using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Methods.Generics
{
   public class BeforeCallMethodCalledParameterNameInGenericsParameterWithRealTypeClassTest :
      NetAspectTest<BeforeCallMethodCalledParameterNameInGenericsParameterWithRealTypeClassTest.MyAspect>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(null, MyAspect.ParameterName);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved();
            Assert.AreEqual("13", MyAspect.ParameterName);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public string Method<T>(T param1)
         {
            return "Hello";
         }

         public string Weaved()
         {
            return Method("13");
         }
      }

      public class MyAspect : Attribute
      {
         public static object ParameterName;
         public bool NetAspectAttribute = true;

         public void BeforeCallMethod(object param1)
         {
            ParameterName = param1;
         }
      }
   }
}
