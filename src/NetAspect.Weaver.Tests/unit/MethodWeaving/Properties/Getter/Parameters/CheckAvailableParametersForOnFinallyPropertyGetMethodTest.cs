using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters
{
    public class CheckAvailableParametersForOnFinallyPropertyGetMethodTest : NetAspectTest<CheckAvailableParametersForOnFinallyPropertyGetMethodTest.MyInt>
   {


        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return errorHandler => errorHandler.Add(
               new ErrorReport.Error
               {
                   Level = ErrorLevel.Error,
                   Message = string.Format("The parameter 'unknown' in the interceptor OnFinallyPropertyGetMethod of the aspect {0} is unknown. Expected one of : instance, property, linenumber, columnnumber, filename, filepath", typeof(LogAttribute).FullName)
               });
        }

      public class MyInt
      {
         private readonly int value;

         public MyInt(int value)
         {
            this.value = value;
         }

         [Log]
         public int Value
         {
            get { return value; }
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

         public void OnFinallyPropertyGetMethod(object unknown)
         {
         }
      }
   }
}
