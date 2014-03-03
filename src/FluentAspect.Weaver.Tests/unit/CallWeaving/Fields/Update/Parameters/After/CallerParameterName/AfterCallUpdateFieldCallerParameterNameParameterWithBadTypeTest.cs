using System;
using FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Instance;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Update.Parameters.After.CallerParameterName
{
    public class AfterCallUpdateFieldCallerParameterNameParameterWithBadTypeTest : NetAspectTest<AfterCallUpdateFieldCallerParameterNameParameterWithBadTypeTest.ClassToWeave>
   {

        protected override Action<FluentAspect.Weaver.Core.Errors.ErrorHandler> CreateErrorHandlerProvider()
        {
            return errorHandler => errorHandler.Errors.Add(string.Format("the instance parameter in the method After of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Object or {1}", typeof(AfterMethodInstanceParameterWithBadTypeTest.MyAspect).FullName, typeof(AfterMethodInstanceParameterWithBadTypeTest.ClassToWeave).FullName));
        }

      public class ClassToWeave
      {

          [MyAspect]
          public string Field;

         public void Weaved(int param1)
         {
             Field = "Hello";
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public void AfterUpdateField(string callerParam1)
         {
         }
      }
   }

   
}