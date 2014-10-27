using System;
using System.Collections.Generic;
using System.Reflection;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.Method
{
   public class AfterCallMethodMethodParameterWithRealTypeReferencedTest :
      NetAspectTest<AfterCallMethodMethodParameterWithRealTypeReferencedTest.ClassToWeave>
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
                              "impossible to ref/out the parameter 'method' in the method AfterCallMethod of the type '{0}'",
                              typeof(MyAspect).FullName)
                    });
       }

      public class ClassToWeave
      {


         [MyAspect]
         public void Called()
         {

         }

         public static void Create()
         {
            new ClassToWeave().Called();
         }
      }

      public class MyAspect : Attribute
      {
         public static MethodBase Method;
         public bool NetAspectAttribute = true;

         public void AfterCallMethod(ref MethodBase method)
         {
            Method = method;
         }
      }
   }
}
