using System;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Parameters
{
   public class AfterMethodParametersParameterWithRealTypeOutTest : NetAspectTest<AfterMethodParametersParameterWithRealTypeOutTest.ClassToWeave>
   {

      protected override Action<FluentAspect.Weaver.Core.Errors.ErrorHandler> CreateErrorHandlerProvider()
      {
         return errorHandler => errorHandler.Errors.Add(string.Format("impossible to ref/out the parameter 'parameters' in the method After of the type '{0}'", typeof(MyAspect).FullName));
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

         public void After(out object[] parameters)
         {
            parameters = null;
         }
      }
   }

   
}