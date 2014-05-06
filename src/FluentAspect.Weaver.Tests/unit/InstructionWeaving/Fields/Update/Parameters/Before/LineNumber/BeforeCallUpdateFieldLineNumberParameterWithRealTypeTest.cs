using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Update.Parameters.Before.LineNumber
{
    public class BeforeCallUpdateFieldLineNumberParameterWithRealTypeTest :
        NetAspectTest<BeforeCallUpdateFieldLineNumberParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.LineNumber);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.AreEqual(26, MyAspect.LineNumber);
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
            public static int LineNumber;
            public bool NetAspectAttribute = true;

            public void BeforeUpdateField(int lineNumber)
            {
                LineNumber = lineNumber;
            }
        }
    }
}