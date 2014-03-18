using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Properties.Update.Parameters.After.FieldValue
{
    public class AfterCallUpdatePropertyFieldValueParameterWithRealTypeTest :
        NetAspectTest<AfterCallUpdatePropertyFieldValueParameterWithRealTypeTest.ClassToWeave>
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

            public void Weaved()
            {
                Property = "Hello";
            }
        }

        public class MyAspect : Attribute
        {
            public static string FieldValue;
            public bool NetAspectAttribute = true;

            public void AfterUpdateProperty(string fieldValue)
            {
                FieldValue = fieldValue;
            }
        }
    }
}