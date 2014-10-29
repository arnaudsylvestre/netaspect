using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Tests.docs.Documentation.LifeCycles;
using NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.Value;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.FieldValue
{
   public class AfterCallGetFieldValueParameterWithBadTypeTest :
      NetAspectTest<AfterCallGetFieldValueParameterWithBadTypeTest.MyInt>
   {
       public AfterCallGetFieldValueParameterWithBadTypeTest()
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
                              "the fieldValue parameter in the method AfterGetField of the type '{0}' is declared with the type 'System.String' but it is expected to be System.Int32 because the return type of the field value in the type {1}",
                              typeof(LogAttribute).FullName,
                              typeof(MyInt))
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
            string fieldValue)
         {
            Called = true;
         }
      }
   }
}
