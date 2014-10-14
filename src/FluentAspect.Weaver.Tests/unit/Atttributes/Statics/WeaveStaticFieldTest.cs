using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.Atttributes.Statics
{
   public class WeaveStaticFieldTest :
      NetAspectTest<WeaveStaticFieldTest.MyInt>
   {
      public WeaveStaticFieldTest()
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
         [Log] private static int value;

         public MyInt(int value)
         {
             MyInt.value = value;
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

         public void AfterGetField(int callerv,
            MyInt caller,
            int columnNumber,
            int lineNumber,
            string fileName,
            string filePath,
            object[] callerParameters,
            MethodBase callerMethod,
            FieldInfo field)
         {
            Called = true;
            Assert.NotNull(caller);
            Assert.AreEqual(13, columnNumber);
            Assert.AreEqual(37, lineNumber);
            Assert.AreEqual("WeaveStaticFieldTest.cs", fileName);
            Assert.AreEqual(fileName, Path.GetFileName(filePath));
            Assert.AreEqual(1, callerParameters.Length);
            Assert.AreEqual("DivideBy", callerMethod.Name);
            Assert.AreEqual("value", field.Name);
            Assert.AreEqual(6, callerv);
         }
      }
   }
}
