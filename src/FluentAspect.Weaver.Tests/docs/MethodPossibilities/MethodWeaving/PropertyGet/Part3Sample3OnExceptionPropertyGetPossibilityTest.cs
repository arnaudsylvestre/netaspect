using System;
using System.Reflection;
using NetAspect.Weaver.Tests.unit.InstructionWeaving.Methods.Parameters.After.Called;
using NUnit.Framework;
using NetAspect.Weaver.Tests.unit;

namespace NetAspect.Weaver.Tests.docs.MethodPossibilities.Before
{
   public class Part3Sample3OnExceptionPropertyGetPossibilityTest : NetAspectTest<Part3Sample3OnExceptionPropertyGetPossibilityTest.MyInt>
    {
       public Part3Sample3OnExceptionPropertyGetPossibilityTest()
         : base("On exception property get weaving possibilities", "PropertyGetWeavingOnException", "PropertyGetWeaving")
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
                get
                {
                    if (value == 0)
                        throw new NotSupportedException("Must not be 0");
                    return value; 
                }
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
                   try
                   {

                      var myInt = new MyInt(0);
                      var val = myInt.Value;
                      Assert.Fail("Must raise an exception");
                   }
                   catch (NotSupportedException)
                   {
                   }
                   Assert.True(LogAttribute.Called);
                };
        }
        

        public class LogAttribute : Attribute
        {
            public bool NetAspectAttribute = true;
           public static bool Called;

           public void OnExceptionPropertyGetMethod(object instance, PropertyInfo property, Exception exception)
           {
              Called = true;
                Assert.AreEqual(typeof(MyInt), instance.GetType());
                Assert.AreEqual("Value", property.Name);
                Assert.AreEqual("NotSupportedException", exception.GetType().Name);
            }
        }
    }
}