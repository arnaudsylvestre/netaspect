using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.ParameterName
{
   public class AfterMethodParameterNameParameterWithGenericMethodTypeReferencedInInterceptorTest :
      NetAspectTest<AfterMethodParameterNameParameterWithGenericMethodTypeReferencedInInterceptorTest.ClassToWeave>
   {
      protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
      {
         return errorHandler => errorHandler.Add(
            new ErrorReport.Error
            {
               Level = ErrorLevel.Error,
               Message = string.Format("Impossible to ref/out the parameter 'i' in the method AfterMethod of the type '{0}' because the parameter type in the method Weaved of the type {1} is a generic type", typeof(MyAspect), typeof(ClassToWeave))
            });
      }

      public class ClassToWeave
      {
         [MyAspect]
         public void Weaved<T>(T i)
         {
         }
      }

      public class MyAspect : Attribute
      {
         public static object I;
         public bool NetAspectAttribute = true;

         public void AfterMethod(ref object i)
         {
            I = i;
         }
      }
   }
}
