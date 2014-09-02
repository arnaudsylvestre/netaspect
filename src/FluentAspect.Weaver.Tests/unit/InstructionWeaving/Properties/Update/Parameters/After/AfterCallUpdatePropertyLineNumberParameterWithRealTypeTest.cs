using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Update.Parameters.After.LineNumber
{
    public class AfterCallUpdatePropertyLineNumberParameterWithRealTypeTest :
        NetAspectTest<AfterCallUpdatePropertyLineNumberParameterWithRealTypeTest.ClassToWeave>
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
            [MyAspect] public string Property {get;set;}

            public void Weaved()
            {
                Property = "Dummy";
            }
        }

        public class MyAspect : Attribute
        {
            public static int LineNumber;
            public bool NetAspectAttribute = true;

            public void AfterSetProperty(int lineNumber)
            {
                LineNumber = lineNumber;
            }
        }
    }
}