using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.Samples
{
   public class TrimSampleTest :
      NetAspectTest<TrimSampleTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            var classToWeave_L = new ClassToWeave();
             Assert.AreEqual("ToTrim", classToWeave_L.TrimWithNetAspect("ToTrim    "));
         };
      }

      public class ClassToWeave
      {
         public string TrimWithNetAspect([Trim] string param)
         {
             return param;
         }
      }

      public class TrimAttribute : Attribute
      {
         public bool NetAspectAttribute = true;

         public void BeforeMethodForParameter(ref string parameterValue)
         {
             if (string.IsNullOrEmpty(parameterValue))
                 return;
             parameterValue = parameterValue.Trim();
         }
      }
   }
}
