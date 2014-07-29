using System;
using System.Reflection;
using NetAspect.Weaver.Tests.unit.InstructionWeaving.Methods.Parameters.After.Called;
using NUnit.Framework;
using NetAspect.Weaver.Tests.unit;

namespace NetAspect.Weaver.Tests.docs.MethodPossibilities.Before
{
   public class Part3Sample2AfterProeprtyGetPossibilityTest : NetAspectTest<Part3Sample2AfterProeprtyGetPossibilityTest.MyInt>
    {
       public Part3Sample2AfterProeprtyGetPossibilityTest()
         : base("After Property Getter Weaving possibilities", "PropertyGetWeavingAfter", "PropertyGetWeaving")
      {
      }

      public class MyInt
        {
            int value;

            public MyInt(int value)
            {
                this.value = value;
            }

            [Log]
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
                    Assert.AreEqual(24, myInt.Value);
                    Assert.True(LogAttribute.Called);
                };
        }
        

        public class LogAttribute : Attribute
        {
            public bool NetAspectAttribute = true;
           public static bool Called;

           public void AfterPropertyGetMethod(object instance, PropertyInfo property, int result)
           {
              Called = true;
                Assert.AreEqual(typeof(MyInt), instance.GetType());
                Assert.AreEqual("Value", property.Name);
                Assert.AreEqual(24, result);
            }
        }
    }
}