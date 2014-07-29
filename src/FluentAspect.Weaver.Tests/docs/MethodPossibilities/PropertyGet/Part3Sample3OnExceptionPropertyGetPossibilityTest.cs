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
                        throw new Exception("Must not be 0");
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

                      var myInt = new MyInt(24);
                      myInt.DivideBy(0);
                      Assert.Fail("Must raise an exception");
                   }
                   catch (Exception)
                   {
                   }
                   Assert.True(LogAttribute.Called);
                };
        }
        

        public class LogAttribute : Attribute
        {
            public bool NetAspectAttribute = true;
           public static bool Called;

           public void OnExceptionPropertyGet(object instance, MethodBase method, object[] parameters, int v, Exception exception)
           {
              Called = true;
                Assert.AreEqual(typeof(MyInt), instance.GetType());
                Assert.AreEqual("DivideBy", method.Name);
                Assert.AreEqual(1, parameters.Length);
                Assert.AreEqual(0, v);
                Assert.AreEqual("DivideByZeroException", exception.GetType().Name);
            }
        }
    }
}