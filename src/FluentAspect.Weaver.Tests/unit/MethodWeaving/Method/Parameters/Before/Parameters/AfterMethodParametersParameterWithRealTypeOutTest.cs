using System;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.Before.Parameters
{
   public class BeforeMethodParametersParameterWithRealTypeOutTest : NetAspectTest<BeforeMethodParametersParameterWithRealTypeOutTest.ClassToWeave>
   {

      protected override Action<FluentAspect.Weaver.Core.Errors.ErrorHandler> CreateErrorHandlerProvider()
      {
         return errorHandler => errorHandler.Errors.Add(string.Format("impossible to ref/out the parameter 'method' in the method Before of the type '{0}'", typeof(MyAspect).FullName));
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

         public void Before(out object[] method)
         {
            method = null;
         }
      }
   }

   
}