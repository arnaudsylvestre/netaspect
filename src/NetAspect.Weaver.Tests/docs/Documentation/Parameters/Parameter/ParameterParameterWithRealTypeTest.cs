using System;
using System.Reflection;
using NUnit.Framework;
using NetAspect.Weaver.Tests.unit;

namespace NetAspect.Weaver.Tests.docs.Documentation.Parameters.Parameter
{
   public class ParameterParameterWithRealTypeTest :
      NetAspectTest<ParameterParameterWithRealTypeTest.ClassToWeave>
   {
       public ParameterParameterWithRealTypeTest()
            : base("It must be of System.Reflection.ParameterInfo type", "ConstructorWeavingBefore", "ConstructorWeaving")
      {
      }
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspectAttribute.Parameter);
            var classToWeave_L = new ClassToWeave(0, "value");

            Assert.AreEqual("p", MyAspectAttribute.Parameter.Name);
         };
      }

      public class ClassToWeave
      {
         public ClassToWeave(int anotherParameter, [MyAspect] string p)
         {
            p = "";
         }
      }

      public class MyAspectAttribute : Attribute
      {
         public static ParameterInfo Parameter;
         public bool NetAspectAttribute = true;

         public void AfterConstructorForParameter(ParameterInfo parameter)
         {
            Parameter = parameter;
         }
      }
   }
}
