using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.FileName
{
   public class AfterMethodFileNameParameterWithRealTypeReferencedTypeTest :
      NetAspectTest<AfterMethodFileNameParameterWithRealTypeReferencedTypeTest.ClassToWeave>
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
                           "impossible to ref/out the parameter 'fileName' in the method AfterMethod of the type '{0}'",
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
         public bool NetAspectAttribute = true;

         public void AfterMethod(ref string fileName)
         {
         }
      }
   }
}
