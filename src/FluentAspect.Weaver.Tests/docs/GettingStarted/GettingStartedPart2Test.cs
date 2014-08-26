using System;
using System.IO;
using System.Reflection;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.MethodPossibilities.MethodWeaving.Method
{
    public class GettingStartedPart2Test : NetAspectTest<GettingStartedPart2Test.Computer>
    {

        public class Computer
        {
            [Log]
            public int Divide(int a, int b)
            {
                return a / b;
            }
        }

        protected override Action CreateEnsure()
        {
            return () =>
                {
                    LogAttribute.writer = new StringWriter();
                    var computer = new Computer();
                    Assert.AreEqual(2, computer.Divide(12, 6));
                    Assert.AreEqual("Before calling method", LogAttribute.writer.ToString());
                };
        }
        

        public class LogAttribute : Attribute
        {
            public static StringWriter writer;

            public bool NetAspectAttribute = true;

           public void Before()
           {
               writer.Write("Before calling method");
            }
        }
    }
}