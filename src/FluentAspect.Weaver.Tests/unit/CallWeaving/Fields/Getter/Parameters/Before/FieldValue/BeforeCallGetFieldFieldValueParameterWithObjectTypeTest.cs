using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.Before.FieldValue
{
    public class BeforeCallGetFieldFieldValueParameterWithObjectTypeTest :
        NetAspectTest<BeforeCallGetFieldFieldValueParameterWithObjectTypeTest.ClassToWeave>
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
            [MyAspect] public string Field;

            public string Weaved()
            {
                return Field;
            }
        }

        public class MyAspect : Attribute
        {
            public static object FieldValue;
            public bool NetAspectAttribute = true;

            public void BeforeGetField(object fieldValue)
            {
                FieldValue = fieldValue;
            }
        }
    }
}