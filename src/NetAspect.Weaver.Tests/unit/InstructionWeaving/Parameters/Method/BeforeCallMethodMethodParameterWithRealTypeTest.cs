using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.Method
{
   public class BeforeCallMethodMethodParameterWithRealTypeTest :
      NetAspectTest<BeforeCallMethodMethodParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(null, MyAspect.Method);
            ClassToWeave.Create();
            Assert.AreEqual("ClassToWeave", MyAspect.Method.DeclaringType.Name);
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

         public void BeforeCallMethod(MethodInfo method)
         {
             Method = method;
         }
      }
   }
}
