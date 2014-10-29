using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.FieldValue
{
   public class AfterCallGetFieldValueParameterWithRealGenericTypeTest :
      NetAspectTest<AfterCallGetFieldValueParameterWithRealGenericTypeTest.LogAttribute>
   {
       public AfterCallGetFieldValueParameterWithRealGenericTypeTest()
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
                              "the fieldValue parameter in the method AfterGetField of the type '{0}' is declared with the type 'System.Int32' but it is expected to be T because the return type of the field value in the type {1}",
                              typeof(LogAttribute).FullName,
                              typeof(MyInt<>).FullName)
                    });
       }

      public class MyInt<T>
      {
         [Log] private readonly T value;

         public MyInt(T value)
         {
            this.value = value;
         }

         public int DivideBy(int v)
         {
            return int.Parse(value.ToString()) / v;
         }
      }

      public class LogAttribute : Attribute
      {
         public static bool Called;
         public bool NetAspectAttribute = true;

         public void AfterGetField(
            int fieldValue)
         {
            Called = true;
            Assert.AreEqual(12, fieldValue);
         }
      }
   }
}
