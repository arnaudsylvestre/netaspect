using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Update.Parameters.Before.FilePath
{
    public class BeforeCallUpdatePropertyFilePathParameterWithRealTypeTest :
        NetAspectTest<BeforeCallUpdatePropertyFilePathParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(null, MyAspect.FilePath);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.True(MyAspect.FilePath.EndsWith(@"FilePath\BeforeCallUpdatePropertyFilePathParameterWithRealTypeTest.cs"));
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
            public static string FilePath;
            public bool NetAspectAttribute = true;

            public void BeforeSetProperty(string filePath)
            {
                FilePath = filePath;
            }
        }
    }
}