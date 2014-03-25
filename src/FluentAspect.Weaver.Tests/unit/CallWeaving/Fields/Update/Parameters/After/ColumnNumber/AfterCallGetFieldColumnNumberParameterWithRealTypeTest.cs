using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Updater.Parameters.After.ColumnNumber
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
                    Assert.AreEqual(17, MyAspect.ColumnNumber);
                };
        }

        public class ClassToWeave
        {
            [MyAspect] public string Field;

            public void Weaved()
            {
                Field = "Dummy";
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