using System;
using System.Reflection;
using NUnit.Framework;
using NetAspect.Weaver.Tests.unit;

namespace NetAspect.Weaver.Tests.docs.MethodPossibilities.Before
{
    public class BeforeMethodPossibilityTest : NetAspectTest<BeforeMethodPossibilityTest.MyInt>
    {

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
                    Assert.AreEqual(2, myInt.DivideBy(12));
                };
        }
        

        public class LogAttribute : Attribute
        {
            public bool NetAspectAttribute = true;

            public void Before(object instance, MethodBase method, object[] parameters, int v)
            {
                Assert.AreEqual(typeof(MyInt), instance.GetType());
                Assert.AreEqual("DivideBy", method.Name);
                Assert.AreEqual(1, parameters.Length);
                Assert.AreEqual(12, v);
            }
        }
    }
}