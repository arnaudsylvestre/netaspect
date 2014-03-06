using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.Before.ColumnNumber
{
    public class BeforeCallGetFieldColumnNumberParameterWithRealTypeTest :
        NetAspectTest<BeforeCallGetFieldColumnNumberParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.ColumnNumber);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.AreEqual(12, MyAspect.ColumnNumber);
                };
        }

        public class ClassToWeave
        {
            [MyAspect] public string Field;

            public string Weaved()
            {
                return Field;
            }
        }

        public class MyAspect : Attribute
        {
            public static int ColumnNumber;
            public bool NetAspectAttribute = true;

            public void BeforeGetField(int columnNumber)
            {
                ColumnNumber = columnNumber;
            }
        }
    }
}