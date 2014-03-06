using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.ParameterName
{
   public class AfterMethodParameterNameParameterWithGenericMethodTypeReferencedInInterceptorTest : NetAspectTest<AfterMethodParameterNameParameterWithGenericMethodTypeReferencedInInterceptorTest.ClassToWeave>
   {

      protected override Action<FluentAspect.Weaver.Core.Errors.ErrorHandler> CreateErrorHandlerProvider()
      {
         return errorHandler => errorHandler.Errors.Add(string.Format("Impossible to ref a generic parameter"));
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