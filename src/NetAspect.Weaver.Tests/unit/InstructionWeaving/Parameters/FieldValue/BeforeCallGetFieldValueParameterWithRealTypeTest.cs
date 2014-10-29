using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.FieldValue
{
   public class BeforeCallGetFieldValueParameterWithRealTypeTest :
      NetAspectTest<BeforeCallGetFieldValueParameterWithRealTypeTest.MyInt>
   {
      public BeforeCallGetFieldValueParameterWithRealTypeTest()
         : base("Instruction which get field value after weaving possibilities", "GetFieldInstructionWeavingAfter", "InstructionFieldWeaving")
      {
      }


      protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
      {
          return
             errorHandler =>
                errorHandler.Add(
                   new ErrorReport.Error
                   {
                       Level = ErrorLevel.Error,
                       Message =
                          string.Format(
                             "The parameter 'fieldValue' in the interceptor BeforeGetField of the aspect {0} is unknown. Expected one of : instance, field, columnnumber, linenumber, filepath, filename, caller, callerparameters, callermethod",
                             typeof(LogAttribute).FullName)
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

         public void BeforeGetField(
            int fieldValue)
         {
            Called = true;
            Assert.AreEqual(12, fieldValue);
         }
      }
   }
}
