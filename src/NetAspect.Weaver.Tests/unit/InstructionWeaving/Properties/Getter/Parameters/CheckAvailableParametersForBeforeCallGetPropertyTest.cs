using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Getter.Parameters
{
   public class CheckAvailableParametersForBeforeCallGetPropertyTest :
      NetAspectTest<CheckAvailableParametersForBeforeCallGetPropertyTest.MyInt>
   {

      protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
      {
          return errorHandler => errorHandler.Add(
             new ErrorReport.Error
             {
                 Level = ErrorLevel.Error,
                 Message = string.Format("The parameter 'unknown' is unknown. Expected one of : called, property, caller, callerparameters, callermethod, columnnumber, linenumber, filepath, filename")
             });
      }

      public class MyInt
      {
         public MyInt(int value)
         {
            Value = value;
         }

         [Log]
         private int Value { get; set; }

         public int DivideBy(int v)
         {
            return Value / v;
         }
      }

      public class LogAttribute : Attribute
      {
         public static bool Called;
         public bool NetAspectAttribute = true;

         public void BeforeGetProperty(int unknown)
         {
         }
      }
   }
}
