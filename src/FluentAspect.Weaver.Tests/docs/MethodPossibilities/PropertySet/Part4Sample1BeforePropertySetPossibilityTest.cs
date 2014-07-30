using System;
using System.Reflection;
using NetAspect.Weaver.Tests.unit;
using NetAspect.Weaver.Tests.unit.InstructionWeaving.Methods.Parameters.After.Called;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.MethodPossibilities.Before
{
   public class Part4Sample1BeforePropertySetPossibilityTest : NetAspectTest<Part4Sample1BeforePropertySetPossibilityTest.MyInt>
    {
       public Part4Sample1BeforePropertySetPossibilityTest()
         : base("Before Property Setter possibilities", "PropertySetWeavingBefore", "PropertySetWeaving")
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

           public void BeforePropertySetMethod(object instance, PropertyInfo property, int value)
           {
                Called = true;
                Assert.AreEqual(typeof(MyInt), instance.GetType());
                Assert.AreEqual("Value", property.Name);
               Assert.AreEqual(32, value);
            }
        }
    }
}