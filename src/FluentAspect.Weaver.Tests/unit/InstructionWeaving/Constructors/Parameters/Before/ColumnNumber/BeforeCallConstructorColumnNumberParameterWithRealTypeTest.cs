using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Constructors.Parameters.Before.ColumnNumber
{
    public class BeforeCallConstructorColumnNumberParameterWithRealTypeTest :
        NetAspectTest<BeforeCallConstructorColumnNumberParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.ColumnNumber);
                    var classToWeave_L = ClassToWeave.Create();
                    Assert.AreEqual(17, MyAspect.ColumnNumber);
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
            public static int ColumnNumber;
            public bool NetAspectAttribute = true;

            public void BeforeCallConstructor(int columnNumber)
            {
                ColumnNumber = columnNumber;
            }
        }
    }
}