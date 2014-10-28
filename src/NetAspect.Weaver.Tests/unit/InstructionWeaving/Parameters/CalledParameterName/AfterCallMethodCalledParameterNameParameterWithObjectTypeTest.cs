using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.CalledParameterName
{
   public class AfterCallMethodCalledParameterNameParameterWithObjectTypeTest :
      NetAspectTest<AfterCallMethodCalledParameterNameParameterWithObjectTypeTest.ClassToWeave>
   {

      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(null, MyAspect.ParameterName);
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
         public static object ParameterName;
         public bool NetAspectAttribute = true;

         public void AfterCallMethod(object param1)
         {
            ParameterName = param1;
         }
      }
   }
}
