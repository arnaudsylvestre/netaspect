using System;
using System.Reflection;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Update.Parameters.After.Field
{
    public class AfterCallUpdateFieldFieldParameterWithRealTypeTest :
        NetAspectTest<AfterCallUpdateFieldFieldParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(null, MyAspect.Field);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.AreEqual("Field", MyAspect.Field.Name);
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
            public static FieldInfo Field;
            public bool NetAspectAttribute = true;

            public void AfterUpdateField(FieldInfo columnNumber)
            {
                Field = columnNumber;
            }
        }
    }
}