using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Constructors.Parameters.After.CallerMethod
{
   public class AfterCallConstructorCallerMethodParameterWithRealTypeTest :
      NetAspectTest<AfterCallConstructorCallerMethodParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(null, MyAspect.Method);
            ClassToWeave classToWeave_L = ClassToWeave.Create();
            Assert.AreEqual("Create", MyAspect.Method.Name);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public ClassToWeave()
         {
         }

         public static ClassToWeave Create()
         {
            return new ClassToWeave();
         }
      }

      public class MyAspect : Attribute
      {
         public static MethodBase Method;
         public bool NetAspectAttribute = true;

         public void AfterCallConstructor(MethodBase callerMethod)
         {
            Method = callerMethod;
         }
      }
   }
}
