using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Methods.Parameters.After.Called
{
   public class AfterCallMethodCalledParameterWithRealTypeAndSelectorTest :
      NetAspectTest<AfterCallMethodCalledParameterWithRealTypeAndSelectorTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.Called);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved();
            Assert.AreEqual(classToWeave_L, MyAspect.Called);
         };
      }

      public class ClassToWeave
      {
         public string Method()
         {
            return "Hello";
         }

         public string Weaved()
         {
            return Method();
         }
      }

      public class MyAspect : Attribute
      {
         public static ClassToWeave Called;
         public bool NetAspectAttribute = true;

         public void AfterCallMethod(ClassToWeave called)
         {
            Called = called;
         }


         public static bool SelectMethod(MethodInfo method)
         {
            return method.Name == "Method";
         }
      }
   }
}
