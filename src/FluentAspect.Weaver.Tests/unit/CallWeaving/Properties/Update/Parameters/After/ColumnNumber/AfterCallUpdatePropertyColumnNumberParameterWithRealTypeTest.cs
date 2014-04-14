using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Properties.Update.Parameters.After.ColumnNumber
{
    public class AfterCallUpdatePropertyColumnNumberParameterWithRealTypeTest :
        NetAspectTest<AfterCallUpdatePropertyColumnNumberParameterWithRealTypeTest.ClassToWeave>
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
            [MyAspect] public string Property {get;set;}

            public void Weaved()
            {
                Property = "Dummy";
            }
        }

        public class MyAspect : Attribute
        {
            public static int ColumnNumber;
            public bool NetAspectAttribute = true;

            public void AfterUpdateProperty(int columnNumber)
            {
                ColumnNumber = columnNumber;
            }
        }
    }
}