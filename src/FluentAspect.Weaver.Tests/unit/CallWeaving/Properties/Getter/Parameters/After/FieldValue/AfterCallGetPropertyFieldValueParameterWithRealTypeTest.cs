using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Properties.Getter.Parameters.After.FieldValue
{
    public class AfterCallGetPropertyFieldValueParameterWithRealTypeTest :
        NetAspectTest<AfterCallGetPropertyFieldValueParameterWithRealTypeTest.ClassToWeave>
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
            public static string FieldValue;
            public bool NetAspectAttribute = true;

            public void AfterGetProperty(string fieldValue)
            {
                FieldValue = fieldValue;
            }
        }
    }
}