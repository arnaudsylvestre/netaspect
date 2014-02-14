using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Instance
{
   public class AfterMethodInstanceParameterWithReferencedObjectTypeTest : NetAspectTest<AfterMethodInstanceParameterWithReferencedObjectTypeTest.ClassToWeave>
   {
      protected override Action<FluentAspect.Weaver.Core.Errors.ErrorHandler> CreateErrorHandlerProvider()
      {
         return errorHandler => errorHandler.Errors.Add(string.Format("impossible to ref/out the parameter 'instance' in the method After of the type 'FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Instance.AfterMethodInstanceParameterWithReferencedObjectTypeTest+MyAspect'"));
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

         public void After(ref object instance)
         {
            Instance = instance;
         }
      }
   }

   
}