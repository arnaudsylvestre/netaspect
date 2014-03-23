using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Selectors
{
    public class GetFieldWithSelectorFieldNameTest :
        NetAspectTest<GetFieldWithSelectorFieldNameTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Caller);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.AreEqual(classToWeave_L, MyAspect.Caller);
                };
        }

        public class ClassToWeave
        {
            public string Field;

            public string Weaved()
            {
                return Field;
            }
        }

        public class MyAspect : Attribute
        {
            public static ClassToWeave Caller;
            public bool NetAspectAttribute = true;

            public void BeforeGetField(ClassToWeave caller)
            {
                Caller = caller;
            }

            public static bool SelectField(string fieldName)
            {
                return fieldName == "Field";
            }
        }
    }
}