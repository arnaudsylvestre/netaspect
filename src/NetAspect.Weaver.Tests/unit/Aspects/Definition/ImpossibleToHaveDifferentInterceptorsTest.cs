using System;
using System.Collections.Generic;
using NUnit.Framework;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Tests.unit.Aspects.Parameters.Types;

namespace NetAspect.Weaver.Tests.unit.Aspects.Definition
{
   public class ImpossibleToHaveDifferentInterceptorsTest :
      NetAspectTest<ImpossibleToHaveDifferentInterceptorsTest.ClassToWeave>
   {

       protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
       {
           return errorHandler => errorHandler.Add(
              new ErrorReport.Error
              {
                  Level = ErrorLevel.Error,
                  Message = string.Format("The interceptor BeforeMethod and the interceptor BeforeUpdateField can not be in the same aspect ({0})", typeof(MyAspect))
              });
       }

      public class ClassToWeave
      {
         [MyAspect]
         public void Weaved(int i)
         {
         }
      }

      public class MyAspect : Attribute
      {
          public static int I;
          public static int Max;
         public bool NetAspectAttribute = true;

         public void AfterMethod(int i)
         {
             I = i;
         }
         public void AfterConstructor(int i)
         {
             I = i;
         }
      }
   }
}
