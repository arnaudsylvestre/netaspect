using System;
using System.IO;
using System.Reflection;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.Documentation.Interceptors.InstructionWeaving.Constructors
{
   public class Part8Sample2AfterInstructionCallMethodPossibilityTest :
      NetAspectTest<Part8Sample2AfterInstructionCallMethodPossibilityTest.MyInt>
   {
      public Part8Sample2AfterInstructionCallMethodPossibilityTest()
         : base("Instruction which call method Weaving possibilities", "CallMethodInstructionWeavingAfter", "InstructionMethodWeaving")
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

         [Log]
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

         public void AfterCallConstructor(
            MyIntUser caller,
            int columnNumber,
            int lineNumber,
            string fileName,
            string filePath,
            object[] callerParameters,
            object[] calledParameters,
            int calledValue,
            MethodBase callerMethod)
         {
            Called = true;
            Assert.NotNull(caller);
            Assert.AreEqual(13, columnNumber);
            Assert.AreEqual(31, lineNumber);
            Assert.AreEqual("Part8Sample2AfterInstructionCallMethodPossibilityTest.cs", fileName);
            Assert.AreEqual(fileName, Path.GetFileName(filePath));
            Assert.AreEqual(3, callerParameters.Length);
            Assert.AreEqual(1, calledParameters.Length);
            Assert.AreEqual("Compute", callerMethod.Name);
            Assert.AreEqual(12, calledValue);
         }
      }
   }
}
