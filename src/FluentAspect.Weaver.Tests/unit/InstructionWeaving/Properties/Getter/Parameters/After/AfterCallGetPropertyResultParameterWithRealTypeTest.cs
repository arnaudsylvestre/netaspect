using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Getter.Parameters.After.Result
{
    public class AfterCallGetPropertyResultParameterWithRealTypeTest :
        NetAspectTest<AfterCallGetPropertyResultParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(null, MyAspect.Result);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Property = "Hello";
                    classToWeave_L.Weaved();
                    Assert.AreEqual("Hello", MyAspect.Result);
                };
        }

        public class ClassToWeave
        {
           [MyAspect] public string Property {get;set;}

            public string Weaved()
            {
               return Property;
            }
        }

        public class MyAspect : Attribute
        {
            public static string Result;
            public bool NetAspectAttribute = true;

            public void AfterGetProperty(string result)
            {
               Result = result;
            }
        }
    }
}