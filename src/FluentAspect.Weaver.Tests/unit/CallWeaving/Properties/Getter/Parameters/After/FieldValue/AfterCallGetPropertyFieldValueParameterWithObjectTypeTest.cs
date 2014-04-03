using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Properties.Getter.Parameters.After.FieldValue
{
    public class AfterCallGetPropertyFieldValueParameterWithObjectTypeTest :
        NetAspectTest<AfterCallGetPropertyFieldValueParameterWithObjectTypeTest.ClassToWeave>
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
            public string Property { get; set; }

            public string Weaved()
            {
                return Property;
            }
        }

        public class MyAspect : Attribute
        {
            public static object FieldValue;
            public bool NetAspectAttribute = true;

            public void AfterGetProperty(object fieldValue)
            {
                FieldValue = fieldValue;
            }
        }
    }
}