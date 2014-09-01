using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Getter.Parameters.After.Result
{
    public class AfterCallGetFieldResultParameterWithRealTypeTest :
        NetAspectTest<AfterCallGetFieldResultParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(null, MyAspect.Result);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.field = "Hello";
                    classToWeave_L.Weaved();
                    Assert.AreEqual("Hello", MyAspect.Result);
                };
        }

        public class ClassToWeave
        {
           [MyAspect] public string field;

            public string Weaved()
            {
               return field;
            }
        }

        public class MyAspect : Attribute
        {
            public static string Result;
            public bool NetAspectAttribute = true;

            public void AfterGetField(string result)
            {
               Result = result;
            }
        }
    }
}