using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.Method
{
   public class AfterCallMethodMethodParameterWithBadTypeTest :
      NetAspectTest<AfterCallMethodMethodParameterWithBadTypeTest.ClassToWeave>
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
                           "the method parameter in the method AfterCallMethod of the type '{0}' is declared with the type 'System.String' but it is expected to be System.Reflection.MethodInfo",
                           typeof(MyAspect).FullName)
                  });
      }

      public class ClassToWeave
      {
          [MyAspect]
         public void Called()
         {

         }

         public void Create()
         {
            Called();
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public void AfterCallMethod(string method)
         {
         }
      }
   }
}
