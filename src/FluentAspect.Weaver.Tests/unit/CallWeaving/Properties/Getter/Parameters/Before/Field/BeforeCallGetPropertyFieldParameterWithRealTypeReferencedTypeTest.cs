using System;
using System.Reflection;
using FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.Before.Instance;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.Before.Field
{
    public class BeforeCallGetPropertyFieldParameterWithRealTypeReferencedTypeTest : NetAspectTest<BeforeCallGetPropertyFieldParameterWithRealTypeReferencedTypeTest.ClassToWeave>
   {

        protected override Action<FluentAspect.Weaver.Core.Errors.ErrorHandler> CreateErrorHandlerProvider()
        {
            return errorHandler => errorHandler.Errors.Add(string.Format("the instance parameter in the method Before of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Object or {1}", typeof(BeforeMethodInstanceParameterWithBadTypeTest.MyAspect).FullName, typeof(BeforeMethodInstanceParameterWithBadTypeTest.ClassToWeave).FullName));
        }

      public class ClassToWeave
      {

          [MyAspect]
          public string Property {get;set;}

         public string Weaved()
         {
             return Property;
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public void BeforeGetProperty(ref FieldInfo field)
         {
         }
      }
   }

   
}