using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Methods.Parameters.Before.FieldValue
{
    public class BeforeCallMethodFieldValueParameterWithObjectTypeTest :
        NetAspectTest<BeforeCallMethodFieldValueParameterWithObjectTypeTest.ClassToWeave>
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
            public static object FieldValue;
            public bool NetAspectAttribute = true;

            public void BeforeCallMethod(object fieldValue)
            {
                FieldValue = fieldValue;
            }
        }
    }
}