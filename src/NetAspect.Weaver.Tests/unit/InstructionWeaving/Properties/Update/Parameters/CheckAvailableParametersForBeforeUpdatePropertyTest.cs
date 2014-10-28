using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Update.Parameters
{
   public class CheckAvailableParametersForBeforeUpdatePropertyTest :
      NetAspectTest<CheckAvailableParametersForBeforeUpdatePropertyTest.MyInt>
   {
       protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
       {
           return errorHandler => errorHandler.Add(
              new ErrorReport.Error
              {
                  Level = ErrorLevel.Error,
                  Message = string.Format("The parameter 'unknown' in the interceptor BeforeUpdateProperty of the aspect {0} is unknown. Expected one of : instance, property, caller, callerparameters, callermethod, columnnumber, linenumber, filepath, filename, propertyvalue", typeof(LogAttribute).FullName)
              });
       }

      public class MyInt
      {
         [Log]
         private int Value { get; set; }

         public void UpdateValue(int intValue)
         {
            Value = intValue;
         }

         public int DivideBy(int v)
         {
            return Value / v;
         }
      }

      public class LogAttribute : Attribute
      {
         public static bool Called;
         public bool NetAspectAttribute = true;

         public void BeforeUpdateProperty(int unknown)
         {
            
         }
      }
   }
}
