using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.CalledParameterName
{
   public class AfterCallMethodCalledParameterNameParameterWithRealTypeReferencedTest :
      NetAspectTest<AfterCallMethodCalledParameterNameParameterWithRealTypeReferencedTest.ClassToWeave>
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
                           "impossible to ref/out the parameter 'param1' in the method AfterCallMethod of the type '{0}'",
                           typeof (MyAspect).FullName)
                  });
      }

      public class ClassToWeave
      {
         [MyAspect]
         public string Method(int param1)
         {
            return "Hello";
         }

         public string Weaved()
         {
            return Method(12);
         }
      }

      public class MyAspect : Attribute
      {
         public static int ParameterName;
         public bool NetAspectAttribute = true;

         public void AfterCallMethod(ref int param1)
         {
            ParameterName = param1;
         }
      }
   }
}
