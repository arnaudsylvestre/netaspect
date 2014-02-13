using System;
using System.Reflection;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Method
{
   public class AfterMethodMethodInfoParameterWithRealTypeReferencedTest : NetAspectTest<AfterMethodMethodInfoParameterWithRealTypeReferencedTest.ClassToWeave>
   {

      protected override Action<FluentAspect.Weaver.Core.Errors.ErrorHandler> CreateErrorHandlerProvider()
      {
         return errorHandler => errorHandler.Errors.Add(string.Format("impossible to ref/out the parameter 'method' in the method After of the type '{0}'", typeof(MyAspect).FullName));
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

         public static MethodInfo Method;

         public void After(ref MethodInfo method)
         {
            Method = method;
         }
      }
   }

   
}