using System;
using System.Text;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Getter.Visibility
{
    public class MethodWeavingForDocWithStartDivisionAndParametersTest :
        NetAspectTest<MethodWeavingForDocWithStartDivisionAndParametersTest.MyInt>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    LogAttribute.Console = new StringBuilder();
                    MyInt value = new MyInt(6);
                    value.DivideBy(3);
                    Assert.AreEqual("Start Division 6 / 3", LogAttribute.Console.ToString());
                };
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

        public class LogAttribute : Attribute
        {
            public bool NetAspectAttribute = true;

            public static StringBuilder Console;

            public void Before(MyInt instance, int v)
            {
                Console.AppendFormat("Start Division {0} / {1}", instance.Value, v);
            }
        }
    }
}