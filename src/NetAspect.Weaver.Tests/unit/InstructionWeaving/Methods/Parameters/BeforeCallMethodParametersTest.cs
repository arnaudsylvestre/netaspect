using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Methods.Parameters
{
   public class BeforeCallMethodParametersTest :
      NetAspectTest<BeforeCallMethodParametersTest.MyInt>
   {
      public BeforeCallMethodParametersTest()
         : base("Instruction which call method Weaving possibilities", "CallMethodInstructionWeavingBefore", "InstructionMethodWeaving")
      {
      }

      protected override Action CreateEnsure()
      {
         return () =>
         {
            var user = new MyIntUser();
            Assert.AreEqual("Result : 2", user.Compute(12, 6, "Result : {0}"));
            Assert.True(LogAttribute.Called);
         };
      }

      public class MyIntUser
      {
         public string Compute(int value, int dividend, string format)
         {
            var myInt = new MyInt(value);
            int result = myInt.DivideBy(dividend);
            return string.Format(format, result);
         }
      }

      public class MyInt
      {
         private readonly int value;

         public MyInt(int value)
         {
            this.value = value;
         }

         [Log]
         public int DivideBy(int v)
         {
            return value / v;
         }
      }

      public class LogAttribute : Attribute
      {
         public static bool Called;
         public bool NetAspectAttribute = true;

         public void BeforeCallMethod(
            MyIntUser callerInstance,
            MyInt instance,
            int columnNumber,
            int lineNumber,
            string fileName,
            string filePath,
            object[] callerParameters,
            object[] parameters,
            int v,
            MethodBase callerMethod)
         {
            Called = true;
            Assert.NotNull(callerInstance);
            Assert.NotNull(callerInstance);
            Assert.AreEqual(13, columnNumber);
            Assert.AreEqual(31, lineNumber);
            Assert.AreEqual("BeforeCallMethodParametersTest.cs", fileName);
            Assert.AreEqual(fileName, Path.GetFileName(filePath));
            Assert.AreEqual(3, callerParameters.Length);
            Assert.AreEqual(1, parameters.Length);
            Assert.AreEqual("Compute", callerMethod.Name);
            Assert.AreEqual(6, v);
         }
      }
   }
}
