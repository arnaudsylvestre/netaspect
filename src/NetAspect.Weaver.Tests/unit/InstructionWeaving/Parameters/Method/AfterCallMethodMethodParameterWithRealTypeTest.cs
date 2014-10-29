using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.Method
{
   public class AfterCallMethodMethodParameterWithRealTypeTest :
      NetAspectTest<AfterCallMethodMethodParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(null, MyAspect.Method);
            ClassToWeave.Create();
            Assert.AreEqual("Called", MyAspect.Method.Name);
         };
      }

      public class ClassToWeave
      {


         [MyAspect]
         public void Called()
         {

         }

         public static void Create()
         {
            new ClassToWeave().Called();
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
