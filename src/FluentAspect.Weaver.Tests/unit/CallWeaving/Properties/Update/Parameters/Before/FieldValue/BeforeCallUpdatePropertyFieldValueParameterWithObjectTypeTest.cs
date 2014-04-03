using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Properties.Update.Parameters.Before.FieldValue
{
    public class BeforeCallUpdatePropertyFieldValueParameterWithObjectTypeTest :
        NetAspectTest<BeforeCallUpdatePropertyFieldValueParameterWithObjectTypeTest.ClassToWeave>
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
            public static object FieldValue;
            public bool NetAspectAttribute = true;

            public void BeforeUpdateProperty(object fieldValue)
            {
                FieldValue = fieldValue;
            }
        }
    }
}