using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Methods.Parameters.Before.FilePath
{
    public class BeforeCallMethodFilePathParameterWithRealTypeTest :
        NetAspectTest<BeforeCallMethodFilePathParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(null, MyAspect.FilePath);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.True(MyAspect.FilePath.EndsWith(@"FilePath\BeforeCallMethodFilePathParameterWithRealTypeTest.cs"));
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Method()
            {
                return "Hello";
            }

            public string Weaved()
            {
                return Method();
            }
        }

        public class MyAspect : Attribute
        {
            public static string FilePath;
            public bool NetAspectAttribute = true;

            public void BeforeCallMethod(string filePath)
            {
                FilePath = filePath;
            }
        }
    }
}