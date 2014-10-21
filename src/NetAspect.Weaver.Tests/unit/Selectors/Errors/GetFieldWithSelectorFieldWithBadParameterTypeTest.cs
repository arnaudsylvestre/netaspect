using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.Selectors.Errors
{
   public class GetFieldWithSelectorFieldWithBadParameterTypeTest :
      NetAspectTest<GetFieldWithSelectorFieldWithBadParameterTypeTest.ClassToWeave>
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
                           "The parameter field in the method SelectField of the aspect {0} is expected to be System.Reflection.FieldInfo",
                           typeof (MyAspect).FullName)
                  });
      }

      public class ClassToWeave
      {
         public string Field;

         public string Weaved()
         {
            return Field;
         }
      }

      public class MyAspect : Attribute
      {
         public static ClassToWeave Caller;
         public bool NetAspectAttribute = true;

         public void BeforeGetField(ClassToWeave caller)
         {
            Caller = caller;
         }

         public static bool SelectField(string field)
         {
            return true;
         }
      }
   }
}
