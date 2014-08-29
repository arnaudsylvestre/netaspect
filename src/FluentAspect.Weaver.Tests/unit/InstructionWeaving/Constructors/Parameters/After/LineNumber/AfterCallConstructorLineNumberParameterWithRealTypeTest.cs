using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Constructors.Parameters.After.LineNumber
{
    public class AfterCallConstructorLineNumberParameterWithRealTypeTest :
        NetAspectTest<AfterCallConstructorLineNumberParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.LineNumber);
                    var classToWeave_L = ClassToWeave.Create();
                    Assert.AreEqual(30, MyAspect.LineNumber);
                };
        }

        public class ClassToWeave
        {
			[MyAspect]
            public ClassToWeave()
            {
            }

            public static ClassToWeave Create()
            {
                return new ClassToWeave();
            }
        }

        public class MyAspect : Attribute
        {
            public static int LineNumber;
            public bool NetAspectAttribute = true;

            public void AfterCallConstructor(int lineNumber)
            {
                LineNumber = lineNumber;
            }
        }
    }
}