using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.FieldValue
{
   public class AfterCallGetFieldValueParameterWithRealTypeTest :
      NetAspectTest<AfterCallGetFieldValueParameterWithRealTypeTest.MyInt>
   {
      public AfterCallGetFieldValueParameterWithRealTypeTest()
         : base("Instruction which get field value after weaving possibilities", "GetFieldInstructionWeavingAfter", "InstructionFieldWeaving")
      {
      }

      protected override Action CreateEnsure()
      {
         return () =>
         {
            var classToWeave_L = new MyInt(12);
            classToWeave_L.DivideBy(6);
            Assert.True(LogAttribute.Called);
         };
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
            int fieldValue)
         {
            Called = true;
            Assert.AreEqual(12, fieldValue);
         }
      }
   }
}
