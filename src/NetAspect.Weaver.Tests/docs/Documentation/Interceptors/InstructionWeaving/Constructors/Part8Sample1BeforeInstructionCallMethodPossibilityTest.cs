using System;
using System.IO;
using System.Reflection;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.Documentation.Interceptors.InstructionWeaving.Constructors
{
   public class Part8Sample1BeforeInstructionCallMethodPossibilityTest :
      NetAspectTest<Part8Sample1BeforeInstructionCallMethodPossibilityTest.MyInt>
   {
      public Part8Sample1BeforeInstructionCallMethodPossibilityTest()
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

         public void BeforeCallConstructor(
            MyIntUser caller,
            int columnNumber,
            int lineNumber,
            string fileName,
            string filePath,
            object[] callerParameters,
            object[] parameters,
            int value,
            MethodBase callerMethod,
             ConstructorInfo constructor)
         {
             Called = true;
             /*Assert.NotNull(caller);
             Assert.AreEqual(".ctor", constructor.Name);
            Assert.AreEqual(13, columnNumber);
            Assert.AreEqual(31, lineNumber);
            Assert.AreEqual("Part8Sample1BeforeInstructionCallMethodPossibilityTest.cs", fileName);
            Assert.AreEqual(fileName, Path.GetFileName(filePath));
            Assert.AreEqual(3, callerParameters.Length);
            Assert.AreEqual(1, parameters.Length);
            Assert.AreEqual("Compute", callerMethod.Name);
            Assert.AreEqual(12, value);*/
         }
      }
   }
}
