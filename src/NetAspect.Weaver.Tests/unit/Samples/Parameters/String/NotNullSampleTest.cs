using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.Samples
{
   public class NotNullSampleTest :
      NetAspectTest<NotNullSampleTest.ClassToWeave>
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
             classToWeave_L.Check("");
         };
      }

      public class ClassToWeave
      {
         public void Check([NotNull] string param)
         {
             
         }
      }

      public class NotNullAttribute : Attribute
      {
         public bool NetAspectAttribute = true;

         public void BeforeMethodForParameter(ParameterInfo parameter, object parameterValue)
         {
            if (parameterValue == null)
                throw new ArgumentNullException(parameter.Name);
         }
      }
   }
}
