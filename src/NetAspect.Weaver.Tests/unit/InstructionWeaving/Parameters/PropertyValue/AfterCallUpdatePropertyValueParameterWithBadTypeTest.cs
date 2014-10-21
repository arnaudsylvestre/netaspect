using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.PropertyValue
{
   public class AfterCallUpdatePropertyValueParameterWithBadTypeTest :
      NetAspectTest<AfterCallUpdatePropertyValueParameterWithBadTypeTest.ClassToWeave>
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
                           "the propertyValue parameter in the method AfterUpdateProperty of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.String",
                           typeof (MyAspect).FullName,
                           typeof (ClassToWeave).FullName)
                  });
      }


      public class ClassToWeave
      {
         [MyAspect] public string Property { get; set; }

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

         public void AfterUpdateProperty(int propertyValue)
         {
             Value = propertyValue.ToString();
         }
      }
   }
}
