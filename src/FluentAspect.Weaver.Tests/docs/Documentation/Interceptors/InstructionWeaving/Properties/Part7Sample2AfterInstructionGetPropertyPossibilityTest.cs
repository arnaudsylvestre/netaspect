using System;
using System.IO;
using System.Reflection;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.MethodPossibilities.InstructionWeaving.Properties
{
   public class Part7Sample2AfterInstructionGetPropertyPossibilityTest :
      NetAspectTest<Part7Sample2AfterInstructionGetPropertyPossibilityTest.MyInt>
   {
      public Part7Sample2AfterInstructionGetPropertyPossibilityTest()
         : base("Instruction which get property value Weaving possibilities", "GetPropertyInstructionWeavingAfter", "InstructionPropertyWeaving")
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
         public MyInt(int value)
         {
            Value = value;
         }

         [Log]
         private int Value { get; set; }

         public int DivideBy(int v)
         {
            return Value / v;
         }
      }

      public class LogAttribute : Attribute
      {
         public static bool Called;
         public bool NetAspectAttribute = true;

         public void AfterGetProperty(int callerv,
            MyInt caller,
            MyInt called,
            int columnNumber,
            int lineNumber,
            string fileName,
            string filePath,
            object[] callerParameters,
            MethodBase callerMethod,
            PropertyInfo property)
         {
            Called = true;
            Assert.AreEqual(caller, called);
            Assert.NotNull(caller);
            Assert.AreEqual(15, columnNumber);
            Assert.AreEqual(40, lineNumber);
            Assert.AreEqual("Part7Sample2AfterInstructionGetPropertyPossibilityTest.cs", fileName);
            Assert.AreEqual(fileName, Path.GetFileName(filePath));
            Assert.AreEqual(1, callerParameters.Length);
            Assert.AreEqual("DivideBy", callerMethod.Name);
            Assert.AreEqual("Value", property.Name);
            Assert.AreEqual(6, callerv);
         }
      }
   }
}
