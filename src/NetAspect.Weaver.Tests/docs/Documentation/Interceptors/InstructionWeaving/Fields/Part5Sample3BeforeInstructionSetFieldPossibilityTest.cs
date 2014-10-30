using System;
using System.IO;
using System.Reflection;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.Documentation.Interceptors.InstructionWeaving.Fields
{
   public class Part5Sample3BeforeInstructionSetFieldPossibilityTest :
      NetAspectTest<Part5Sample3BeforeInstructionSetFieldPossibilityTest.MyInt>
   {
      public Part5Sample3BeforeInstructionSetFieldPossibilityTest()
         : base("Instruction which set field value before Weaving possibilities", "SetFieldInstructionWeavingBefore", "InstructionFieldWeaving")
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

         public void BeforeUpdateField(
            MyInt callerInstance,
            MyInt instance,
            int columnNumber,
            int lineNumber,
            string fileName,
            string filePath,
            object[] callerParameters,
            MethodBase callerMethod,
            FieldInfo field,
             int newFieldValue)
         {
            Called = true;
            Assert.AreEqual(callerInstance, instance);
            Assert.NotNull(callerInstance);
            Assert.AreEqual(13, columnNumber);
            Assert.AreEqual(33, lineNumber);
            Assert.AreEqual("Part5Sample3BeforeInstructionSetFieldPossibilityTest.cs", fileName);
            Assert.AreEqual(fileName, Path.GetFileName(filePath));
            Assert.AreEqual(1, callerParameters.Length);
            Assert.AreEqual("UpdateValue", callerMethod.Name);
            Assert.AreEqual("value", field.Name);
            Assert.AreEqual(6, newFieldValue);
         }
      }
   }
}
