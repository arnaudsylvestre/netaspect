using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.ParameterName
{
   public class AfterMethodParameterNameParameterWithGenericMethodTypeReferencedInInterceptorTest : NetAspectTest<AfterMethodParameterNameParameterWithGenericMethodTypeReferencedInInterceptorTest.ClassToWeave>
   {

      protected override Action<FluentAspect.Weaver.Core.Errors.ErrorHandler> CreateErrorHandlerProvider()
      {
         return errorHandler => errorHandler.Errors.Add(string.Format("the parameters parameter in the method After of the type '{0}' is declared with the type 'System.Int32' but it is expected to be {1}", typeof(MyAspect).FullName, typeof(object[])));
      }

      public class ClassToWeave
      {
         [MyAspect]
         public void Weaved<T>(T i)
         {

         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static object I;

         public void After(ref object i)
         {
            I = i;
         }
      }
   }

   
}