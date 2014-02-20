using System;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.OnException.Exceptions
{
   public class OnExceptionMethodExceptionParameterWithExceptionTypeReferencedTest : NetAspectTest<OnExceptionMethodExceptionParameterWithExceptionTypeReferencedTest.ClassToWeave>
   {
      protected override Action<FluentAspect.Weaver.Core.Errors.ErrorHandler> CreateErrorHandlerProvider()
      {
         return errorHandler => errorHandler.Errors.Add(string.Format("impossible to ref/out the parameter 'exception' in the method OnException of the type '{0}'", typeof(MyAspect).FullName));
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
         public bool NetAspectAttribute = true;

         public void OnException(ref Exception exception)
         {
         }
      }
   }

   
}