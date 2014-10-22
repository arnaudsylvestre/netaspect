using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Update.Parameters
{
   public class AfterUpdateFieldParametersTest :
      NetAspectTest<AfterUpdateFieldParametersTest.MyInt>
   {
      public AfterUpdateFieldParametersTest()
         : base("Instruction which set field value after Weaving possibilities", "SetFieldInstructionWeavingAfter", "InstructionFieldWeaving")
      {
      }

      protected override Action CreateEnsure()
      {
         return () =>
         {
            var classToWeave_L = new MyInt();
            classToWeave_L.UpdateValue(6);
            Assert.True(LogAttribute.Called);
         };
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

         public void AfterUpdateField(
            MyInt caller,
            MyInt called,
            int columnNumber,
            int lineNumber,
            string fileName,
            string filePath,
            object[] callerParameters,
            MethodBase callerMethod,
            FieldInfo field)
         {
            Called = true;
            Assert.AreEqual(caller, called);
            Assert.NotNull(caller);
            Assert.AreEqual(13, columnNumber);
            Assert.AreEqual(32, lineNumber);
            Assert.AreEqual("AfterUpdateFieldParametersTest.cs", fileName);
            Assert.AreEqual(fileName, Path.GetFileName(filePath));
            Assert.AreEqual(1, callerParameters.Length);
            Assert.AreEqual("UpdateValue", callerMethod.Name);
            Assert.AreEqual("value", field.Name);
         }
      }
   }
}
