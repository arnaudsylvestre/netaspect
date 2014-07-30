using System;
using System.Reflection;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.MethodPossibilities.Constructor
{
   public class Part2Sample4OnFinallyConstructorPossibilityTest : NetAspectTest<Part2Sample4OnFinallyConstructorPossibilityTest.MyInt>
    {
      public Part2Sample4OnFinallyConstructorPossibilityTest()
         : base("On finally constructor weaving possibilities", "ConstructorWeavingOnFinally", "ConstructorWeaving")
      {
      }

      public class MyInt
        {
            int value;

            [Log]
            public MyInt(int intValue)
            {
               this.value = intValue;
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
                      myInt.DivideBy(12);
                      Assert.True(LogAttribute.Called);
                };
        }
        

        public class LogAttribute : Attribute
        {
            public bool NetAspectAttribute = true;
           public static bool Called;

           public void OnFinallyConstructor(object instance, MethodBase constructor, object[] parameters, int intValue)
           {
                Called = true;
                Assert.AreEqual(typeof(MyInt), instance.GetType());
                Assert.AreEqual(".ctor", constructor.Name);
                Assert.AreEqual(1, parameters.Length);
                Assert.AreEqual(24, intValue);
            }
        }
    }
}