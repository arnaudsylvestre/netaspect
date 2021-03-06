using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.Value
{
   public class AfterCallUpdateNewFieldValueParameterWithBadTypeTest :
      NetAspectTest<AfterCallUpdateNewFieldValueParameterWithBadTypeTest.ClassToWeave>
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
                           "the newFieldValue parameter in the method AfterUpdateField of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.String",
                           typeof (MyAspect).FullName,
                           typeof (ClassToWeave).FullName)
                  });
      }


      public class ClassToWeave
      {
         [MyAspect] public string field;

         public string Weaved()
         {
            field = "Hello";
            return field;
         }
      }

      public class MyAspect : Attribute
      {
         public static string Value;
         public bool NetAspectAttribute = true;

         public void AfterUpdateField(int newFieldValue)
         {
             Value = newFieldValue.ToString();
         }
      }
   }
}
