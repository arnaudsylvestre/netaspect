using System;
using System.IO;
using System.Reflection;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.Documentation.Interceptors.InstructionWeaving.Properties
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

         public void AfterGetProperty(
            MyInt callerInstance,
            MyInt instance,
            int columnNumber,
            int lineNumber,
            string fileName,
            string filePath,
            object[] callerParameters,
            MethodBase callerMethod,
            PropertyInfo property,
             int propertyValue)
         {
            Called = true;
            Assert.AreEqual(callerInstance, instance);
            Assert.NotNull(callerInstance);
            Assert.AreEqual(13, columnNumber);
            Assert.AreEqual(39, lineNumber);
            Assert.AreEqual("Part7Sample2AfterInstructionGetPropertyPossibilityTest.cs", fileName);
            Assert.AreEqual(fileName, Path.GetFileName(filePath));
            Assert.AreEqual(1, callerParameters.Length);
            Assert.AreEqual("DivideBy", callerMethod.Name);
            Assert.AreEqual("Value", property.Name);
            Assert.AreEqual(12, propertyValue);
         }
      }
   }
}
