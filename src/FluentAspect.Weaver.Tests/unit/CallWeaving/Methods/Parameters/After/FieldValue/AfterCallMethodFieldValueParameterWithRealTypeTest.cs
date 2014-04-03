using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Methods.Parameters.After.FieldValue
{
    public class AfterCallMethodFieldValueParameterWithRealTypeTest :
        NetAspectTest<AfterCallMethodFieldValueParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.FieldValue);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.AreEqual(classToWeave_L, MyAspect.FieldValue);
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
            public static string FieldValue;
            public bool NetAspectAttribute = true;

            public void AfterCallMethod(string fieldValue)
            {
                FieldValue = fieldValue;
            }
        }
    }
}