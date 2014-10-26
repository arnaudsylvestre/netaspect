using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters
{
   public class CheckAvailableParametersForOnFinallyPropertySetMethodTest : NetAspectTest<CheckAvailableParametersForOnFinallyPropertySetMethodTest.MyInt>
   {


       protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
       {
           return errorHandler => errorHandler.Add(
              new ErrorReport.Error
              {
                  Level = ErrorLevel.Error,
                  Message = string.Format("The parameter 'unknown' in the interceptor OnFinallyPropertySetMethod of the aspect {0} is unknown. Expected one of : instance, property, propertyvalue, linenumber, columnnumber, filename, filepath", typeof(LogAttribute).FullName)
              });
       }

      public class MyInt
      {
         private int value;

         public MyInt(int value)
         {
            this.value = value;
         }

         [Log]
         public int Value
         {
            set { this.value = value; }
         }

         public int DivideBy(int v)
         {
            return value / v;
         }
      }


      public class LogAttribute : Attribute
      {
         public static bool Called;
         public bool NetAspectAttribute = true;

         public void OnFinallyPropertySetMethod(object unknown)
         {
         }
      }
   }
}
