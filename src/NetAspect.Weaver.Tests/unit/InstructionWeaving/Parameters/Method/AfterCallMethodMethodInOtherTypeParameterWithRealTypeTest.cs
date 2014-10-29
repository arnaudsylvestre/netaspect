using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.Method
{
   public class AfterCallMethodMethodInOtherTypeParameterWithRealTypeTest :
      NetAspectTest<AfterCallMethodMethodInOtherTypeParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(null, MyAspect.Method);
            var classToWeave_L = new ClassToWeave();
            Assert.AreEqual(2, classToWeave_L.Weaved().Value);
            Assert.AreEqual("Called", MyAspect.Method.Name);
         };
      }

      public class ClassCalled
      {
          public int Value = 2;

         [MyAspect]
         public void Called()
         {
             
         }
      }

      public class ClassToWeave
      {
         private readonly ClassCalled called;

         public ClassCalled Weaved()
         {
            var classCalled_L = new ClassCalled();
             classCalled_L.Called();
            return classCalled_L;
         }
      }

      public class MyAspect : Attribute
      {
          public static MethodBase Method;
         public bool NetAspectAttribute = true;

         public void AfterCallMethod(MethodInfo method)
         {
             Method = method;
         }
      }
   }
}
