using System;
using System.IO;
using System.Reflection;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.Documentation.Interceptors.InstructionWeaving.Properties
{
   public class Part7Sample3BeforeInstructionSetPropertyPossibilityTest :
      NetAspectTest<Part7Sample3BeforeInstructionSetPropertyPossibilityTest.MyInt>
   {
      public Part7Sample3BeforeInstructionSetPropertyPossibilityTest()
         : base("Instruction which set property value before Weaving possibilities", "SetPropertyInstructionWeavingBefore", "InstructionPropertyWeaving")
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
         [Log]
         private int Value { get; set; }

         public void UpdateValue(int intValue)
         {
            Value = intValue;
         }

         public int DivideBy(int v)
         {
            return Value / v;
         }
      }

      public class LogAttribute : Attribute
      {
         public static bool Called;
         public bool NetAspectAttribute = true;

         public void BeforeUpdateProperty(
            MyInt caller,
            MyInt instance,
            int columnNumber,
            int lineNumber,
            string fileName,
            string filePath,
            object[] callerParameters,
            MethodBase callerMethod,
            PropertyInfo property,
            int newPropertyValue)
         {
            Called = true;
            Assert.AreEqual(caller, instance);
            Assert.NotNull(caller);
            Assert.AreEqual(13, columnNumber);
            Assert.AreEqual(34, lineNumber);
            Assert.AreEqual("Part7Sample3BeforeInstructionSetPropertyPossibilityTest.cs", fileName);
            Assert.AreEqual(fileName, Path.GetFileName(filePath));
            Assert.AreEqual(1, callerParameters.Length);
            Assert.AreEqual("UpdateValue", callerMethod.Name);
            Assert.AreEqual("Value", property.Name);
            Assert.AreEqual(6, newPropertyValue);
         }
      }
   }
}
