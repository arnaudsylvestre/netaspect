using System;
using System.Reflection;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.MethodPossibilities.Constructor
{
   public class Part2Sample1BeforeConstructorPossibilityTest : NetAspectTest<Part2Sample1BeforeConstructorPossibilityTest.MyInt>
    {
      public Part2Sample1BeforeConstructorPossibilityTest()
         : base("Before Method Weaving possibilities", "MethodWeavingBefore", "MethodWeaving")
      {
      }


      public class MyInt
        {
            int value;
         
            [Log]
            public MyInt(int value)
            {
                this.value = value;
            }

            public int Value
            {
                get { return value; }
            }
            
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

            public void BeforeConstructor(object instance, MethodBase constructor, object[] parameters, int value)
            {
                Assert.AreEqual(typeof(MyInt), instance.GetType());
                Assert.AreEqual(".ctor", constructor.Name);
                Assert.AreEqual(1, parameters.Length);
                Assert.AreEqual(24, value);
            }
        }
    }
}