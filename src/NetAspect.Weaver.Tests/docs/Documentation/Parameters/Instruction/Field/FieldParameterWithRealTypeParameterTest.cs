using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Getter.Parameters
{
   public class FieldParameterWithRealTypeParameterTest :
      NetAspectTest<FieldParameterWithRealTypeParameterTest.MyInt>
   {
      public FieldParameterWithRealTypeParameterTest()
           : base("It must be of System.Reflection.FieldInfo type", "GetFieldInstructionWeavingBefore", "InstructionFieldWeaving")
      {
      }

      protected override Action CreateEnsure()
      {
         return () =>
         {
            var classToWeave_L = new MyInt(12);
            classToWeave_L.DivideBy(6);
            Assert.AreEqual("value", LogAttribute.Field.Name);
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
          public static FieldInfo Field;
         public bool NetAspectAttribute = true;

         public void BeforeGetField(
            FieldInfo field)
         {
             Field = field;
         }
      }
   }
}
