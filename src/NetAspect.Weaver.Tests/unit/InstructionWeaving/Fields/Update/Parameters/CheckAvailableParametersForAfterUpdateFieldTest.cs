using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Update.Parameters
{
   public class CheckAvailableParametersForAfterUpdateFieldTest :
      NetAspectTest<CheckAvailableParametersForAfterUpdateFieldTest.MyInt>
   {
       protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
       {
           return errorHandler => errorHandler.Add(
              new ErrorReport.Error
              {
                  Level = ErrorLevel.Error,
                  Message = string.Format("The parameter 'unknown' in the interceptor AfterUpdateField of the aspect {0} is unknown. Expected one of : instance, field, columnnumber, linenumber, filepath, filename, caller, callerparameters, callermethod, newfieldvalue", typeof(LogAttribute).FullName)
              });
       }

      public class MyInt
      {
         [Log] private int value;

         public void UpdateValue(int intValue)
         {
            value = intValue;
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

         public void AfterUpdateField(int unknown)
         {
         }
      }
   }
}
