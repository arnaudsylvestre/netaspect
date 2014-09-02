using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.LineNumber
{
    public class AfterCallMethodLineNumberParameterWithRealTypeTest :
        NetAspectTest<AfterCallMethodLineNumberParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.LineNumber);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.AreEqual(30, MyAspect.LineNumber);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Method()
            {
                return "Hello";
            }

            public string Weaved()
            {
                return Method();
            }
        }

        public class MyAspect : Attribute
        {
            public static int LineNumber;
            public bool NetAspectAttribute = true;

            public void AfterCallMethod(int lineNumber)
            {
                LineNumber = lineNumber;
            }
        }
    }
}