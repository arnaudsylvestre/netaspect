using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.Method
{
   public class BeforeCallMethodMethodParameterWithOverloadingRealTypeTest :
      NetAspectTest<BeforeCallMethodMethodParameterWithOverloadingRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(null, MyAspect.Method);
            ClassToWeave.Create();
            Assert.AreEqual("ClassToWeave", MyAspect.Method.DeclaringType.Name);
            Assert.AreEqual("Called", MyAspect.Method.Name);
            Assert.AreEqual(1, MyAspect.Method.GetParameters().Length);
         };
      }

      public class ClassToWeave
      {


          [MyAspect]
          public void Called()
          {

          }


          [MyAspect]
          public void Called(int value)
          {

          }

         public static void Create()
         {
            new ClassToWeave().Called(3);
         }
      }

      public class MyAspect : Attribute
      {
         public static MethodBase Method;
         public bool NetAspectAttribute = true;

         public void BeforeCallMethod(MethodBase method)
         {
             Method = method;
         }
      }
   }
}
