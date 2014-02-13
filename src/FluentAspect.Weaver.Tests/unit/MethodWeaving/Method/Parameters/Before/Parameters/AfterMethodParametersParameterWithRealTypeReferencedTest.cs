using System;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.Before.Parameters
{
   public class BeforeMethodParametersParameterWithRealTypeReferencedTest : NetAspectTest<BeforeMethodParametersParameterWithRealTypeReferencedTest.ClassToWeave>
   {

      protected override Action<FluentAspect.Weaver.Core.Errors.ErrorHandler> CreateErrorHandlerProvider()
      {
         return errorHandler => errorHandler.Errors.Add(string.Format("impossible to ref/out the parameter 'parameters' in the method Before of the type '{0}'", typeof(MyAspect).FullName));
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

         public static object[] Method;

         public void Before(ref object[] method)
         {
            Method = method;
         }
      }
   }

   
}