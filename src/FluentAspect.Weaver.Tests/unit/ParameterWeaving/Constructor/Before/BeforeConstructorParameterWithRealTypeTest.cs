using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.Before.Instance
{
   public class BeforeConstructorParameterWithRealTypeTest :
      NetAspectTest<BeforeConstructorParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.Parameter);
            var classToWeave_L = new ClassToWeave(0, "value");
            Assert.AreEqual("p", MyAspect.Parameter.Name);
         };
      }

      public class ClassToWeave
      {
         public ClassToWeave(int anotherParameter, [MyAspect] string p)
         {
            p = "";
         }
      }

      public class MyAspect : Attribute
      {
         public static ParameterInfo Parameter;
         public bool NetAspectAttribute = true;

         public void BeforeConstructorForParameter(ParameterInfo parameter)
         {
            Parameter = parameter;
         }
      }
   }
}
