using System;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.OnException.Instance
{
   public class OnExceptionMethodInstanceParameterWithRealTypeOutTest : NetAspectTest<OnExceptionMethodInstanceParameterWithRealTypeOutTest.ClassToWeave>
   {

      protected override Action<FluentAspect.Weaver.Core.Errors.ErrorHandler> CreateErrorHandlerProvider()
      {
         return errorHandler => errorHandler.Errors.Add(string.Format("impossible to ref/out the parameter 'instance' in the method OnException of the type '{0}'", typeof(MyAspect).FullName));
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

         public void OnException(out ClassToWeave instance)
         {
            instance = null;
         }
      }
   }

   
}