using System;
using System.Text;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Getter.Visibility
{
    public class MethodWeavingForDocTest :
        NetAspectTest<MethodWeavingForDocTest.MyInt>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
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
        }
    }
}