using System;
using FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Instance;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.After.Called
{
    public class AfterCallUpdatePropertyCalledParameterWithRealTypeReferencedTypeTest : NetAspectTest<AfterCallUpdatePropertyCalledParameterWithRealTypeReferencedTypeTest.ClassToWeave>
   {

        protected override Action<FluentAspect.Weaver.Core.Errors.ErrorHandler> CreateErrorHandlerProvider()
        {
            return errorHandler => errorHandler.Errors.Add(string.Format("the instance parameter in the method After of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Object or {1}", typeof(AfterMethodInstanceParameterWithBadTypeTest.MyAspect).FullName, typeof(AfterMethodInstanceParameterWithBadTypeTest.ClassToWeave).FullName));
        }

      public class ClassToWeave
      {

          [MyAspect]
          public string Property {get;set;}

         public void Weaved()
         {
             Property = "Hello";
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public void AfterUpdateProperty(ref ClassToWeave called)
         {
         }
      }
   }

   
}