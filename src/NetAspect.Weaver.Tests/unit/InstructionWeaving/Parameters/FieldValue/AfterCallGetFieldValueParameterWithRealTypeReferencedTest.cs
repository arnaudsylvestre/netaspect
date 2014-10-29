using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.Value;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.FieldValue
{
   public class AfterCallGetFieldValueParameterWithRealTypeReferencedTest :
      NetAspectTest<AfterCallGetFieldValueParameterWithRealTypeReferencedTest.MyInt>
   {
       public AfterCallGetFieldValueParameterWithRealTypeReferencedTest()
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
                              "impossible to ref/out the parameter 'fieldValue' in the method AfterGetField of the type '{0}'",
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

         public void AfterGetField(
            ref int fieldValue)
         {
            Called = true;
            Assert.AreEqual(12, fieldValue);
         }
      }
   }
}
