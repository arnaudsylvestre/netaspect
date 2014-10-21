using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.Instance
{
   public class AfterMethodInstanceParameterWithReferencedObjectTypeTest :
      NetAspectTest<AfterMethodInstanceParameterWithReferencedObjectTypeTest.ClassToWeave>
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
                           "impossible to ref/out the parameter 'instance' in the method AfterMethod of the type '{0}'",
                           typeof (MyAspect))
                  });
      }

      public class ClassToWeave
      {
         [MyAspect]
         public void Weaved()
         {
         }
      }

      public class MyAspect : Attribute
      {
         public static object Instance;
         public bool NetAspectAttribute = true;

         public void AfterMethod(ref object instance)
         {
            Instance = instance;
         }
      }
   }
}
