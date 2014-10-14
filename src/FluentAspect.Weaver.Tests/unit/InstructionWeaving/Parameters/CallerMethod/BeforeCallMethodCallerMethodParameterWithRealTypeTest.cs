using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.CallerMethod
{
   public class BeforeCallMethodCallerMethodParameterWithRealTypeTest :
      NetAspectTest<BeforeCallMethodCallerMethodParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(null, MyAspect.Method);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved();
            Assert.AreEqual("Weaved", MyAspect.Method.Name);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
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
         public static MethodBase Method;
         public bool NetAspectAttribute = true;

         public void BeforeCallMethod(MethodBase callerMethod)
         {
            Method = callerMethod;
         }
      }
   }
}
