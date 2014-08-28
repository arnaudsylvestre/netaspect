using System;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.Documentation.Parameters
{
   public class InstanceParameterWithRealTypeParameterTest : NetAspectTest<InstanceParameterWithRealTypeParameterTest.MyInt>
    {
      public InstanceParameterWithRealTypeParameterTest()
           : base("This method is executed before the code of the method is executed", "MethodWeavingBefore", "MethodWeaving")
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
           public static MyInt Instance;

           public void Before(MyInt instance)
           {
              Instance = instance;
           }
        }
    }
}