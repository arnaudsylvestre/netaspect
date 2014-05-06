using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Getter.Parameters.Before.FilePath
{
    public class BeforeCallGetPropertyFilePathParameterWithRealTypeTest :
        NetAspectTest<BeforeCallGetPropertyFilePathParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(null, MyAspect.FilePath);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.True(MyAspect.FilePath.EndsWith(@"FilePath\BeforeCallGetPropertyFilePathParameterWithRealTypeTest.cs"));
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
            public static string FilePath;
            public bool NetAspectAttribute = true;

            public void BeforeGetProperty(string filePath)
            {
                FilePath = filePath;
            }
        }
    }
}