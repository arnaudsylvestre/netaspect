using System;
using FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.Before.Instance;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Update.Parameters.Before.CalledParameters
{
    public class BeforeCallUpdateFieldCalledParametersParameterWithObjectTypeTest : NetAspectTest<BeforeCallUpdateFieldCalledParametersParameterWithObjectTypeTest.ClassToWeave>
    {
        protected override Action<FluentAspect.Weaver.Core.Errors.ErrorHandler> CreateErrorHandlerProvider()
        {
            return errorHandler => errorHandler.Errors.Add(string.Format("the instance parameter in the method Before of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Object or {1}", typeof(BeforeMethodInstanceParameterWithBadTypeTest.MyAspect).FullName, typeof(BeforeMethodInstanceParameterWithBadTypeTest.ClassToWeave).FullName));
        }

      public class ClassToWeave
      {

          [MyAspect]
          public string Field;

         public void Weaved()
         {
             Field = "Hello";
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public void BeforeUpdateField(object calledParameters)
         {
         }
      }
   }

   
}