using System;
using FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.Before.Instance;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Events.Subsribe.Parameters.Before.Caller
{
    public class BeforeCallSubscribeEventCallerParameterWithBadTypeTest : NetAspectTest<BeforeCallSubscribeEventCallerParameterWithBadTypeTest.ClassToWeave>
   {

        protected override Action<FluentAspect.Weaver.Core.Errors.ErrorHandler> CreateErrorHandlerProvider()
        {
            return errorHandler => errorHandler.Errors.Add(string.Format("the instance parameter in the method Before of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Object or {1}", typeof(BeforeMethodInstanceParameterWithBadTypeTest.MyAspect).FullName, typeof(BeforeMethodInstanceParameterWithBadTypeTest.ClassToWeave).FullName));
        }

      public class ClassToWeave
      {

          [MyAspect]
          public event Action Event;

         public void Weaved()
         {
             Event += () => {};
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static int Caller;

         public void BeforeRaiseEvent(int caller)
         {
             Caller = caller;
         }
      }
   }

   
}