using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Getter.Parameters
{
   public class CheckAvailableParametersForAfterGetFieldTest :
      NetAspectTest<CheckAvailableParametersForAfterGetFieldTest.MyInt>
   {

       protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
       {
           return errorHandler => errorHandler.Add(
              new ErrorReport.Error
              {
                  Level = ErrorLevel.Error,
                  Message = string.Format("The parameter 'unknown' is unknown. Expected one of : called, field, columnnumber, linenumber, filepath, filename, caller, callerparameters, callermethod, result")
              });
       }

      public class MyInt
      {
         [Log] private readonly int value;

         public MyInt(int value)
         {
            this.value = value;
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

         public void AfterGetField(int unknown)
         {
         }
      }
   }
}
