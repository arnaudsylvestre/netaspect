using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Constructors.Parameters.After.ColumnNumber
{
    public class AfterCallConstructorPdbParameterWithRealTypeTest :
        NetAspectTest<AfterCallConstructorPdbParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.ColumnNumber);
                    var classToWeave_L = ClassToWeave.Create();
                    Assert.AreEqual(17, MyAspect.ColumnNumber);
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
           public static int ColumnNumber;
           public static int LineNumber;
            public bool NetAspectAttribute = true;

            public void AfterCallConstructor(int columnNumber, int lineNumber)
            {
               LineNumber = lineNumber;
               ColumnNumber = columnNumber;
            }
        }
    }
}