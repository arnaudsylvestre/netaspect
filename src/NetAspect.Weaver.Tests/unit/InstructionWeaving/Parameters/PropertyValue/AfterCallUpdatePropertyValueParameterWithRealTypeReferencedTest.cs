using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.PropertyValue
{
   public class AfterCallUpdatePropertyValueParameterWithRealTypeReferencedTest :
      NetAspectTest<AfterCallUpdatePropertyValueParameterWithRealTypeReferencedTest.ClassToWeave>
   {
      protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
      {
         return
            errorHandler =>
               errorHandler.Add(
                  new ErrorReport.Error
                  {
                     Level = ErrorLevel.Error,
                     Message =
                        string.Format(
                           "impossible to ref/out the parameter 'newPropertyValue' in the method AfterUpdateProperty of the type '{0}'",
                           typeof (MyAspect).FullName)
                  });
      }

      public class ClassToWeave
      {
          [MyAspect]
          public string Property { get; set; }

         public string Weaved()
         {
            Property = "Hello";
            return Property;
         }
      }

      public class MyAspect : Attribute
      {
         public static string Value;
         public bool NetAspectAttribute = true;

         public void AfterUpdateProperty(ref string newPropertyValue)
         {
             Value = newPropertyValue;
             newPropertyValue = "New Hello";
         }
      }
   }
}
