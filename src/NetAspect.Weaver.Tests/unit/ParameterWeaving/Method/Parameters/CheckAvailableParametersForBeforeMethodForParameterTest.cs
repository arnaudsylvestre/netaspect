using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.ParameterWeaving.Method.Parameters
{
   public class CheckAvailableParametersForBeforeMethodForParameterTest :
      NetAspectTest<CheckAvailableParametersForBeforeMethodForParameterTest.MyInt>
   {


       protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
       {
           return errorHandler => errorHandler.Add(
              new ErrorReport.Error
              {
                  Level = ErrorLevel.Error,
                  Message = string.Format("The parameter 'unknown' is unknown. Expected one of : parametervalue, parameter, constructor, parameters, v, instance")
              });
       }

      public class MyInt
      {
         private readonly int value;

         public MyInt(int value)
         {
            this.value = value;
         }

         public int DivideBy([Log] int v)
         {
            return value / v;
         }
      }

      public class LogAttribute : Attribute
      {
         public static bool Called;
         public bool NetAspectAttribute = true;

         public void BeforeMethodForParameter(int unknown)
         {
            
         }
      }
   }
}
