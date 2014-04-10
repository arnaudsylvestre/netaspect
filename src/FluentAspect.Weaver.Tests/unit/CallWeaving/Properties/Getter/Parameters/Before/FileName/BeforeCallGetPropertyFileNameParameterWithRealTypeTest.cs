using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Properties.Getter.Parameters.Before.FileName
{
    public class BeforeCallGetPropertyFileNameParameterWithRealTypeTest :
        NetAspectTest<BeforeCallGetPropertyFileNameParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(null, MyAspect.FileName);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.AreEqual("BeforeCallGetPropertyFileNameParameterWithRealTypeTest.cs", MyAspect.FileName);
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

            public void BeforeGetProperty(string fileName)
            {
                FileName = fileName;
            }
        }
    }
}