using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Update.Parameters.Before.Field
{
    public class BeforeCallUpdateFieldFieldParameterWithRealTypeTest :
        NetAspectTest<BeforeCallUpdateFieldFieldParameterWithRealTypeTest.ClassToWeave>
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
                Field = "Dummy";
            }
        }

        public class MyAspect : Attribute
        {
            public static FieldInfo Field;
            public bool NetAspectAttribute = true;

            public void BeforeUpdateField(FieldInfo field)
            {
                Field = field;
            }
        }
    }
}