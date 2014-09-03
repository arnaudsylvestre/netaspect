using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.Result
{
   public class AfterMethodResultParameterWithReferencedObjectTest :
      NetAspectTest<AfterMethodResultParameterWithReferencedObjectTest.ClassToWeave>
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
                           "the result parameter in the method After of the type '{0}' is declared with the type 'System.Object&' but it is expected to be System.String because the return type of the method Weaved in the type {1}",
                           typeof (MyAspect).FullName,
                           typeof (ClassToWeave).FullName)
                  });
      }


      public class ClassToWeave
      {
         [MyAspect]
         public string Weaved()
         {
            return "NeverUsedValue";
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;


         public void After(ref object result)
         {
         }
      }
   }
}
