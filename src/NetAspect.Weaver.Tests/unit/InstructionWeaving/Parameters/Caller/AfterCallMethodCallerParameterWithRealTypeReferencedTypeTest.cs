using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.Caller
{
   public class AfterCallMethodCallerParameterWithRealTypeReferencedTypeTest :
      NetAspectTest<AfterCallMethodCallerParameterWithRealTypeReferencedTypeTest.ClassToWeave>
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
                           "impossible to ref/out the parameter 'callerInstance' in the method AfterCallMethod of the type '{0}'",
                           typeof (MyAspect).FullName,
                           typeof (ClassToWeave).FullName)
                  });
      }

      public class ClassToWeave
      {
         [MyAspect]
         public string Method()
         {
            return "Hello";
         }

         public string Weaved()
         {
            return Method();
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public void AfterCallMethod(ref ClassToWeave callerInstance)
         {
         }
      }
   }
}
