using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Update.Selectors
{
    public class UpdateFieldWithSelectorFieldNameTest :
        NetAspectTest<UpdateFieldWithSelectorFieldNameTest.ClassToWeave>
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

            public void Weaved()
            {
                Field = "Dummy";
            }
        }

        public class MyAspect : Attribute
        {
            public static ClassToWeave Caller;
            public bool NetAspectAttribute = true;

            public void BeforeUpdateField(ClassToWeave caller)
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