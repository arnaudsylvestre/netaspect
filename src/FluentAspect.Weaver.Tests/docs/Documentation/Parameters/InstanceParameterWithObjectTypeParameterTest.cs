using System;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.Documentation.Parameters
{
   public class InstanceParameterWithObjectTypeParameterTest : NetAspectTest<InstanceParameterWithObjectTypeParameterTest.MyInt>
    {
      public InstanceParameterWithObjectTypeParameterTest()
           : base("It can be declared as object", "MethodWeavingBefore", "MethodWeaving")
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
                    var myInt = new MyInt(24);
                    Assert.AreEqual(2, myInt.DivideBy(12));
                    Assert.AreSame(myInt, LogAttribute.Instance);
                };
        }
        

        public class LogAttribute : Attribute
        {
            public bool NetAspectAttribute = true;
           public static object Instance;

           public void Before(object instance)
           {
              Instance = instance;
           }
        }
    }
}