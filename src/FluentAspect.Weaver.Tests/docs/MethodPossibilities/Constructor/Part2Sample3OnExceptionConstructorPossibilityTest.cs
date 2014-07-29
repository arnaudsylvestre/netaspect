using System;
using System.Reflection;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.MethodPossibilities.Constructor
{
   public class Part2Sample3OnExceptionConstructorPossibilityTest : NetAspectTest<Part2Sample3OnExceptionConstructorPossibilityTest.MyInt>
    {
      public Part2Sample3OnExceptionConstructorPossibilityTest()
         : base("On exception method weaving possibilities", "MethodWeavingOnException", "MethodWeaving")
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
                   try
                   {

                      var myInt = new MyInt(24);
                      myInt.DivideBy(0);
                      Assert.Fail("Must raise an exception");
                   }
                   catch (Exception)
                   {
                   }
                };
        }
        

        public class LogAttribute : Attribute
        {
            public bool NetAspectAttribute = true;

            public void OnException(object instance, MethodBase method, object[] parameters, int v, Exception exception)
            {
                Assert.AreEqual(typeof(MyInt), instance.GetType());
                Assert.AreEqual("DivideBy", method.Name);
                Assert.AreEqual(1, parameters.Length);
                Assert.AreEqual(0, v);
                Assert.AreEqual("DivideByZeroException", exception.GetType().Name);
            }
        }
    }
}