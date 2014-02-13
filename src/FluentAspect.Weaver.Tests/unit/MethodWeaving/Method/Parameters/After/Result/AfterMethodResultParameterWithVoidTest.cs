using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Result
{
    public class AfterMethodResultParameterWithVoidTest : NetAspectTest<AfterMethodResultParameterWithVoidTest.ClassToWeave>
   {
      protected override Action<FluentAspect.Weaver.Core.Errors.ErrorHandler> CreateErrorHandlerProvider()
      {
         return errorHandler => errorHandler.Errors.Add(string.Format("the result parameter in the method After of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Object or {1}", typeof(MyAspect).FullName, typeof(ClassToWeave).FullName));
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

         public void After(int result)
         {
         }
      }
   }

   
}