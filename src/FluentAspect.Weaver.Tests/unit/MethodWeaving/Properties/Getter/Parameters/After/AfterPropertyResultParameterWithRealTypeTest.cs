using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters.After.Result
{
   public class AfterPropertyResultParameterWithRealTypeTest :
      NetAspectTest<AfterPropertyResultParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            var classToWeave_L = new ClassToWeave();
            string res = classToWeave_L.Weaved;
            Assert.AreEqual("NeverUsedValue", res);
            Assert.AreEqual("NeverUsedValue", MyAspect.Result);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public string Weaved
         {
            get { return "NeverUsedValue"; }
         }
      }

      public class MyAspect : Attribute
      {
         public static string Result;
         public bool NetAspectAttribute = true;

         public void AfterPropertyGetMethod(string result)
         {
            Result = result;
            result = "MyNewValue";
         }
      }
   }
}
