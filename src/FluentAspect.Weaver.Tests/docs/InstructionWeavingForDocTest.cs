using System;
using System.Text;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Getter.Visibility
{
    public class InstructionWeavingForDocTest :
        NetAspectTest<InstructionWeavingForDocTest.MyInt>
    {
        protected override Action CreateEnsure()
        {
            return () =>
            {
               Computer.Divide(6, 3);
               Assert.AreEqual(44, LogAttribute.LineNumber);
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

       public class Computer
       {
          public static int Divide(int a, int b)
          {
             MyInt myInt = new MyInt(a);
             return myInt.DivideBy(b);
          }

       }

        public class LogAttribute : Attribute
        {
            public bool NetAspectAttribute = true;

           public static int LineNumber;

           public void BeforeCallMethod(int lineNumber)
           {
              LineNumber = lineNumber;
           }
        }
    }
}