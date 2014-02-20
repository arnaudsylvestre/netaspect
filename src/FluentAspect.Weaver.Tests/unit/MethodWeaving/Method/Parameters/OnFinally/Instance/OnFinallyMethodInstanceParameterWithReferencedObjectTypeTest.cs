using System;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.OnFinally.Instance
{
   public class OnFinallyMethodInstanceParameterWithReferencedObjectTypeTest : NetAspectTest<OnFinallyMethodInstanceParameterWithReferencedObjectTypeTest.ClassToWeave>
   {
      protected override Action<FluentAspect.Weaver.Core.Errors.ErrorHandler> CreateErrorHandlerProvider()
      {
         return errorHandler => errorHandler.Errors.Add(string.Format("impossible to ref/out the parameter 'instance' in the method OnFinally of the type 'FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.OnFinally.Instance.OnFinallyMethodInstanceParameterWithReferencedObjectTypeTest+MyAspect'"));
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

         public static object Instance;

         public void OnFinally(ref object instance)
         {
            Instance = instance;
         }
      }
   }

   
}