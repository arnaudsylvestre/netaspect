using System;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.OnException.Instance
{
   public class OnExceptionMethodInstanceParameterWithReferencedObjectTypeTest : NetAspectTest<OnExceptionMethodInstanceParameterWithReferencedObjectTypeTest.ClassToWeave>
   {
      protected override Action<FluentAspect.Weaver.Core.Errors.ErrorHandler> CreateErrorHandlerProvider()
      {
         return errorHandler => errorHandler.Errors.Add(string.Format("impossible to ref/out the parameter 'instance' in the method OnException of the type 'FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.OnException.Instance.OnExceptionMethodInstanceParameterWithReferencedObjectTypeTest+MyAspect'"));
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

         public void OnException(ref object instance)
         {
            Instance = instance;
         }
      }
   }

   
}