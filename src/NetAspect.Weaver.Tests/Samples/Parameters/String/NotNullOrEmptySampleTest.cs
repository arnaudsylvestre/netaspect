using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.Samples
{
   public class NotNullOrEmptySampleTest :
      NetAspectTest<NotNullOrEmptySampleTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            var classToWeave_L = new ClassToWeave();
             try
             {
                 classToWeave_L.Check(null);
                 Assert.Fail("Must fail");
             }
             catch (ArgumentNullException e)
             {
                 Assert.AreEqual("param", e.ParamName);
             }
             try
             {
                 classToWeave_L.Check("");
                 Assert.Fail("Must fail");
             }
             catch (ArgumentNullException e)
             {
                 Assert.AreEqual("param", e.ParamName);
             }
             classToWeave_L.Check("not empty");
         };
      }

      public class ClassToWeave
      {
         public void Check([NotNullOrEmpty] string param)
         {
             
         }
      }

      public class NotNullOrEmptyAttribute : Attribute
      {
         public bool NetAspectAttribute = true;

         public void BeforeMethodForParameter(ParameterInfo parameter, string parameterValue)
         {
             if (string.IsNullOrEmpty(parameterValue))
                 throw new ArgumentNullException(parameter.Name);

         }
      }
   }
}
