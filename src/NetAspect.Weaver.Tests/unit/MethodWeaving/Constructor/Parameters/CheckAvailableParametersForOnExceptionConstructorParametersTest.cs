using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters
{
    public class CheckAvailableParametersForOnExceptionConstructorParametersTest : NetAspectTest<CheckAvailableParametersForOnExceptionConstructorParametersTest.MyInt>
   {

        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return errorHandler => errorHandler.Add(
               new ErrorReport.Error
               {
                   Level = ErrorLevel.Error,
                   Message = string.Format("The parameter 'unknown' in the interceptor OnExceptionConstructor of the aspect {0} is unknown. Expected one of : instance, constructor, parameters, intvalue, linenumber, columnnumber, filename, filepath, exception", typeof(LogAttribute).FullName)
               });
        }


      public class MyInt
      {
         private readonly int value;

         [Log]
         public MyInt(int intValue)
         {
            value = intValue;
         }

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

         public void OnExceptionConstructor(object unknown)
         {
         }
      }
   }
}
