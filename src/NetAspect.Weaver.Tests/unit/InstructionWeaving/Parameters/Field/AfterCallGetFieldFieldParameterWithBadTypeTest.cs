using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.Field
{
   public class AfterCallGetFieldFieldParameterWithBadTypeTest :
      NetAspectTest<AfterCallGetFieldFieldParameterWithBadTypeTest.ClassToWeave>
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
                           "the field parameter in the method AfterGetField of the type '{0}' is declared with the type 'System.String' but it is expected to be System.Reflection.FieldInfo",
                           typeof (MyAspect).FullName,
                           typeof (ClassToWeave).FullName)
                  });
      }

      public class ClassToWeave
      {
         [MyAspect] public string Field;

         public string Weaved()
         {
            return Field;
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public void AfterGetField(string field)
         {
         }
      }
   }
}
