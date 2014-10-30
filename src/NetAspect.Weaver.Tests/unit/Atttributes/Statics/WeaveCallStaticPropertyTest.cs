using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Getter.Parameters;

namespace NetAspect.Weaver.Tests.unit.Atttributes.Statics
{
   public class WeaveCallStaticPropertyTest :
      NetAspectTest<WeaveCallStaticPropertyTest.MyInt>
   {
       public WeaveCallStaticPropertyTest()
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
         private static int Value { get; set; }

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
            int columnNumber,
            int lineNumber,
            string fileName,
            string filePath,
            object[] callerParameters,
            MethodBase callerMethod,
            PropertyInfo property)
         {
            Called = true;
            Assert.NotNull(callerInstance);
            Assert.AreEqual(13, columnNumber);
            Assert.AreEqual(39, lineNumber);
            Assert.AreEqual("WeaveCallStaticPropertyTest.cs", fileName);
            Assert.AreEqual(fileName, Path.GetFileName(filePath));
            Assert.AreEqual(1, callerParameters.Length);
            Assert.AreEqual("DivideBy", callerMethod.Name);
            Assert.AreEqual("Value", property.Name);
         }
      }
   }
}
