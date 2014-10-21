using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters
{
    public class CheckAvailableParametersForAfterPropertyGetMethodTest : NetAspectTest<CheckAvailableParametersForAfterPropertyGetMethodTest.MyInt>
   {


        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return errorHandler => errorHandler.Add(
               new ErrorReport.Error
               {
                   Level = ErrorLevel.Error,
                   Message = string.Format("The parameter 'unknown' is unknown. Expected one of : instance, property, linenumber, columnnumber, filename, filepath, result")
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

         public void AfterPropertyGetMethod(object unknown)
         {
         }
      }
   }
}
