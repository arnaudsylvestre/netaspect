using System;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.OnFinally.Instance
{
   public class OnFinallyMethodInstanceParameterWithRealTypeReferencedTest : NetAspectTest<OnFinallyMethodInstanceParameterWithRealTypeReferencedTest.ClassToWeave>
   {

      protected override Action<FluentAspect.Weaver.Core.Errors.ErrorHandler> CreateErrorHandlerProvider()
      {
         return errorHandler => errorHandler.Errors.Add(string.Format("impossible to ref/out the parameter 'instance' in the method OnFinally of the type '{0}'", typeof(MyAspect).FullName));
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

         public static ClassToWeave Instance;

         public void OnFinally(ref ClassToWeave instance)
         {
            Instance = instance;
         }
      }
   }

   
}