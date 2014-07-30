using System;
using System.Reflection;
using NetAspect.Weaver.Tests.unit.InstructionWeaving.Methods.Parameters.After.Called;
using NUnit.Framework;
using NetAspect.Weaver.Tests.unit;

namespace NetAspect.Weaver.Tests.docs.MethodPossibilities.Before
{
   public class Part4Sample2AfterPropertySetPossibilityTest : NetAspectTest<Part4Sample2AfterPropertySetPossibilityTest.MyInt>
    {
       public Part4Sample2AfterPropertySetPossibilityTest()
         : base("After Property Setter Weaving possibilities", "PropertySetWeavingAfter", "PropertySetWeaving")
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
                set { this.value = value; }
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
                    myInt.Value = 32;
                    Assert.True(LogAttribute.Called);
                };
        }
        

        public class LogAttribute : Attribute
        {
            public bool NetAspectAttribute = true;
           public static bool Called;

           public void AfterPropertySetMethod(object instance, PropertyInfo property, int value)
           {
              Called = true;
                Assert.AreEqual(typeof(MyInt), instance.GetType());
                Assert.AreEqual("Value", property.Name);
                Assert.AreEqual(32, value);
            }
        }
    }
}