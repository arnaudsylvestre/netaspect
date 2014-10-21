using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.Selectors.Members
{
   public class WeaveParameterWithSelectorTest : NetAspectTest<WeaveParameterWithSelectorTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.Parameter);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved(1, "value");
            Assert.AreEqual("p", MyAspect.Parameter.Name);
         };
      }

      public class ClassToWeave
      {
         public void Weaved(int anotherParameter, string p)
         {
            p = "";
         }
      }

      public class MyAspect : Attribute
      {
         public static ParameterInfo Parameter;
         public bool NetAspectAttribute = true;

         public void BeforeMethodForParameter(ParameterInfo parameter)
         {
            Parameter = parameter;
         }


         public static bool SelectParameter(ParameterInfo parameter)
         {
            return parameter.Name == "p";
         }
      }
   }
}
