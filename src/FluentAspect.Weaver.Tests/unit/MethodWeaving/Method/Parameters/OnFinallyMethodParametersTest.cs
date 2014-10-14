using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters
{
   public class OnFinallyMethodParametersTest : NetAspectTest<OnFinallyMethodParametersTest.MyInt>
   {
      public OnFinallyMethodParametersTest()
         : base("This method is executed after the method is executed if an exception is raised or not", "MethodWeavingOnFinally", "MethodWeaving")
      {
      }

      public class MyInt
      {
         private readonly int value;

         public MyInt(int value)
         {
            this.value = value;
         }

         public int Value
         {
            get { return value; }
         }

         [Log]
         public int DivideBy(int v)
         {
            return value / v;
         }
      }

      protected override Action CreateEnsure()
      {
         return () =>
         {
            var myInt = new MyInt(24);
            myInt.DivideBy(12);
            Assert.True(LogAttribute.Called);
         };
      }


      public class LogAttribute : Attribute
      {
         public static bool Called;
         public bool NetAspectAttribute = true;

         public void OnFinallyMethod(object instance, MethodBase method, object[] parameters, int v, string fileName, string filePath, int lineNumber, int columnNumber)
         {
            Called = true;
            Assert.AreEqual(typeof (MyInt), instance.GetType());
            Assert.AreEqual("DivideBy", method.Name);
            Assert.AreEqual(1, parameters.Length);
            Assert.AreEqual(12, v);
         }
      }
   }
}
