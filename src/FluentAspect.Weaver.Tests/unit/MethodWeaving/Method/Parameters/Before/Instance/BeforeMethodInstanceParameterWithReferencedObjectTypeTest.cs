using System;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.Before.Instance
{
   public class BeforeMethodInstanceParameterWithReferencedObjectTypeTest : NetAspectTest<BeforeMethodInstanceParameterWithReferencedObjectTypeTest.ClassToWeave>
   {
      protected override Action<FluentAspect.Weaver.Core.Errors.ErrorHandler> CreateErrorHandlerProvider()
      {
         return errorHandler => errorHandler.Errors.Add(string.Format("impossible to ref/out the parameter 'instance' in the method Before of the type 'FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.Before.Instance.BeforeMethodInstanceParameterWithReferencedObjectTypeTest+MyAspect'"));
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

         public void Before(ref object instance)
         {
            Instance = instance;
         }
      }
   }

   
}