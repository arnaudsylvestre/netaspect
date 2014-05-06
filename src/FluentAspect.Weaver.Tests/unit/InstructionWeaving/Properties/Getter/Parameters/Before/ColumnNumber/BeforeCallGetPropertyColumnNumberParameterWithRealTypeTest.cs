using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Getter.Parameters.Before.ColumnNumber
{
    public class BeforeCallGetPropertyColumnNumberParameterWithRealTypeTest :
        NetAspectTest<BeforeCallGetPropertyColumnNumberParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.ColumnNumber);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.AreEqual(17, MyAspect.ColumnNumber);
                };
        }

        public class ClassToWeave
        {
            [MyAspect] public string Property {get; set; }

            public string Weaved()
            {
                return Property;
            }
        }

        public class MyAspect : Attribute
        {
            public static int ColumnNumber;
            public bool NetAspectAttribute = true;

            public void BeforeGetProperty(int columnNumber)
            {
                ColumnNumber = columnNumber;
            }
        }
    }
}