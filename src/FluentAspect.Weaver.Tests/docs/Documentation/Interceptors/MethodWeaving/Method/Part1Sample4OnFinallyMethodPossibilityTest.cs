using System;
using System.Reflection;
using NUnit.Framework;
using NetAspect.Weaver.Tests.unit;

namespace NetAspect.Weaver.Tests.docs.MethodPossibilities.MethodWeaving.Method
{
   public class Part1Sample4OnFinallyMethodPossibilityTest : NetAspectTest<Part1Sample4OnFinallyMethodPossibilityTest.MyInt>
    {
      public Part1Sample4OnFinallyMethodPossibilityTest()
           : base("This method is executed after the method is executed if an exception is raised or not", "MethodWeavingOnFinally", "MethodWeaving")
      {
      }

      public class MyInt
        {
            int value;

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
            public bool NetAspectAttribute = true;
           public static bool Called;

           public void OnFinally(object instance, MethodBase method, object[] parameters, int v)
           {
              Called = true;
                Assert.AreEqual(typeof(MyInt), instance.GetType());
                Assert.AreEqual("DivideBy", method.Name);
                Assert.AreEqual(1, parameters.Length);
                Assert.AreEqual(12, v);
            }
        }
    }
}