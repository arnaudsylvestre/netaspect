using System;
using System.Reflection;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.MethodPossibilities.Constructor
{
   public class Part2Sample4OnFinallyConstructorPossibilityTest : NetAspectTest<Part2Sample4OnFinallyConstructorPossibilityTest.MyInt>
    {
      public Part2Sample4OnFinallyConstructorPossibilityTest()
         : base("On finally method weaving possibilities", "MethodWeavingOnFinally", "MethodWeaving")
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
                };
        }
        

        public class LogAttribute : Attribute
        {
            public bool NetAspectAttribute = true;

            public void OnFinally(object instance, MethodBase method, object[] parameters, int v)
            {
                Assert.AreEqual(typeof(MyInt), instance.GetType());
                Assert.AreEqual("DivideBy", method.Name);
                Assert.AreEqual(1, parameters.Length);
                Assert.AreEqual(12, v);
            }
        }
    }
}