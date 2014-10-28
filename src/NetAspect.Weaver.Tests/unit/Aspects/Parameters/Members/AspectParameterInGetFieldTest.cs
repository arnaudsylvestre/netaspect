using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.Aspects.Parameters.Members
{
   public class AspectParameterInGetFieldTest :
      NetAspectTest<AspectParameterInGetFieldTest.MyInt>
   {
      public AspectParameterInGetFieldTest()
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
         [Log(3)] private readonly int value;

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
          private int max;

          public LogAttribute(int max)
          {
              this.max = max;
          }

          public void AfterGetField(
            MyInt caller,
            MyInt instance,
            int columnNumber,
            int lineNumber,
            string fileName,
            string filePath,
            object[] callerParameters,
            MethodBase callerMethod,
            FieldInfo field)
         {
            Called = true;
            Assert.AreEqual(caller, instance);
            Assert.NotNull(caller);
            Assert.AreEqual(13, columnNumber);
            Assert.AreEqual(37, lineNumber);
            Assert.AreEqual("AspectParameterInGetFieldTest.cs", fileName);
            Assert.AreEqual(fileName, Path.GetFileName(filePath));
            Assert.AreEqual(1, callerParameters.Length);
            Assert.AreEqual("DivideBy", callerMethod.Name);
            Assert.AreEqual("value", field.Name);
             Assert.AreEqual(3, max);
         }
      }
   }
}
