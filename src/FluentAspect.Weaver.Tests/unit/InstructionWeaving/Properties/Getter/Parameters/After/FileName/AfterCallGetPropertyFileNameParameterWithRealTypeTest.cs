using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Getter.Parameters.After.FileName
{
    public class AfterCallGetPropertyFileNameParameterWithRealTypeTest :
        NetAspectTest<AfterCallGetPropertyFileNameParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(null, MyAspect.FileName);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.AreEqual("AfterCallGetPropertyFileNameParameterWithRealTypeTest.cs", MyAspect.FileName);
                };
        }

        public class ClassToWeave
        {
            [MyAspect] public string Property {get; set; }

            public string Weaved()
            {
                return Property;
            }
        }

        public class MyAspect : Attribute
        {
            public static string FileName;
            public bool NetAspectAttribute = true;

            public void AfterGetProperty(string fileName)
            {
                FileName = fileName;
            }
        }
    }
}