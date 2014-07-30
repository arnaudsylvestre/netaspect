using System;
using System.Reflection;
using NetAspect.Weaver.Tests.unit.InstructionWeaving.Methods.Parameters.After.Called;
using NUnit.Framework;
using NetAspect.Weaver.Tests.unit;

namespace NetAspect.Weaver.Tests.docs.MethodPossibilities.Before
{
   public class Part4Sample4OnFinallyPropertySetPossibilityTest : NetAspectTest<Part4Sample4OnFinallyPropertySetPossibilityTest.MyInt>
    {
       public Part4Sample4OnFinallyPropertySetPossibilityTest()
           : base("On finally property setter weaving possibilities", "PropertySetWeavingOnFinally", "PropertySetWeaving")
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
                      myInt.Value = 12;
                      Assert.True(LogAttribute.Called);
                };
        }
        

        public class LogAttribute : Attribute
        {
            public bool NetAspectAttribute = true;
           public static bool Called;

           public void OnFinallyPropertySetMethod(object instance, PropertyInfo property, int value)
           {
              Called = true;
              Assert.AreEqual(typeof(MyInt), instance.GetType());
              Assert.AreEqual("Value", property.Name);
              Assert.AreEqual(12, value);
            }
        }
    }
}