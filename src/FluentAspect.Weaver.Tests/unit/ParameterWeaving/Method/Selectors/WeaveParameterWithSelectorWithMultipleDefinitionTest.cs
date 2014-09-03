using System;
using System.Collections.Generic;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.ParameterWeaving.Method.Selectors
{
   public class WeaveParameterWithSelectorWithMultipleDefinitionTest : NetAspectTest<WeaveParameterWithSelectorWithMultipleDefinitionTest.ClassToWeave>
   {
      protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
      {
         return
            errorHandler =>
               errorHandler.Add(
                  new ErrorReport.Error
                  {
                     Level = ErrorLevel.Error,
                     Message =
                        string.Format(
                           "Only one SelectParameter can be defined in the aspect {0}",
                           typeof (MyAspect).FullName)
                  });
      }

      public class ClassToWeave
      {
         public void Weaved(int anotherParameter, string p)
         {
            p = "";
         }
      }

      public class MyAspect : Attribute
      {
         public static ParameterInfo Parameter;
         public bool NetAspectAttribute = true;

         public void BeforeMethodForParameter(ParameterInfo parameter)
         {
            Parameter = parameter;
         }

         public static bool SelectParameter(ParameterInfo unknown)
         {
            return false;
         }

         public static bool SelectParameter(ParameterInfo unknown, int other)
         {
            return false;
         }
      }
   }
}
