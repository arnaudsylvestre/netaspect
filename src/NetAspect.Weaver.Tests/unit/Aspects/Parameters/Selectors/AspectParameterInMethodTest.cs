using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Tests.unit.Aspects.Parameters.Types;

namespace NetAspect.Weaver.Tests.unit.Aspects.Parameters.Selectors
{
   public class AspectParameterInMethodTest :
      NetAspectTest<AspectParameterInMethodTest.ClassToWeave>
   {

       protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
       {
           return errorHandler => errorHandler.Add(
              new ErrorReport.Error
              {
                  Level = ErrorLevel.Error,
                  Message = string.Format("The aspect '{0}' must have a constructor with no parameters because he got selectors", typeof(MyAspect))
              });
       }

      public class ClassToWeave
      {
         public void Weaved(int i)
         {
         }
      }

      public class MyAspect : Attribute
      {
          private readonly int _max;
          public static int I;
          public static int Max;
         public bool NetAspectAttribute = true;

          public MyAspect(int max)
          {
              _max = max;
          }

          public void AfterMethod(int i)
         {
            I = i;
              Max = _max;
         }

          public static bool SelectMethod(MethodInfo method)
          {
              return method.Name == "Weaved";
          }
      }
   }
}
