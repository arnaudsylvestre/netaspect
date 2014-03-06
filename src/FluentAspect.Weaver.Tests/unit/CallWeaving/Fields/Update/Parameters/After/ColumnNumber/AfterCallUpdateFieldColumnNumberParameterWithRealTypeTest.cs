using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Update.Parameters.After.ColumnNumber
{
    public class AfterCallUpdateFieldColumnNumberParameterWithRealTypeTest :
        NetAspectTest<AfterCallUpdateFieldColumnNumberParameterWithRealTypeTest.ClassToWeave>
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

            public void Weaved()
            {
                Field = "Hello";
            }
        }

        public class MyAspect : Attribute
        {
            public static int ColumnNumber;
            public bool NetAspectAttribute = true;

            public void AfterUpdateField(int columnNumber)
            {
                ColumnNumber = columnNumber;
            }
        }
    }
}