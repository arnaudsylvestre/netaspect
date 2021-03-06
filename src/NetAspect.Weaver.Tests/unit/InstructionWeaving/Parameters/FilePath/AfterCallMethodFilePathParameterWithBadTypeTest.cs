using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.FilePath
{
   public class AfterCallMethodFilePathParameterWithBadTypeTest :
      NetAspectTest<AfterCallMethodFilePathParameterWithBadTypeTest.ClassToWeave>
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
                           "the filePath parameter in the method AfterCallMethod of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.String",
                           typeof (MyAspect).FullName)
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
         public static int FilePath;
         public bool NetAspectAttribute = true;

         public void AfterCallMethod(int filePath)
         {
            FilePath = filePath;
         }
      }
   }
}
