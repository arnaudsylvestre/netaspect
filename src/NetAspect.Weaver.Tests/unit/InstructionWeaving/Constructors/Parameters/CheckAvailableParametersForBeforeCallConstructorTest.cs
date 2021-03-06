using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Constructors.Parameters
{
   public class CheckAvailableParametersForBeforeCallConstructorTest :
      NetAspectTest<CheckAvailableParametersForBeforeCallConstructorTest.MyInt>
   {
      protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
      {
          return errorHandler => errorHandler.Add(
             new ErrorReport.Error
             {
                 Level = ErrorLevel.Error,
                 Message = string.Format("The parameter 'unknown' in the interceptor BeforeCallConstructor of the aspect {0} is unknown. Expected one of : parameters, value, constructor, callerinstance, callerparameters, callermethod, columnnumber, linenumber, filepath, filename", typeof(LogAttribute).FullName)
             });
      }

      public class MyIntUser
      {
         public string Compute(int value, int dividend, string format)
         {
            var myInt = new MyInt(value);
            int result = myInt.DivideBy(dividend);
            return string.Format(format, result);
         }
      }

      public class MyInt
      {
         private readonly int value;

         [Log]
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
         public bool NetAspectAttribute = true;

         public void BeforeCallConstructor(string unknown)
         {
         }
      }
   }
}
