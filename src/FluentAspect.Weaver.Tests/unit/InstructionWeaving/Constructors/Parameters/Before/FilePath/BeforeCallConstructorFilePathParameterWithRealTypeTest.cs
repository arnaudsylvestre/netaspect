using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Constructors.Parameters.Before.FilePath
{
    public class BeforeCallConstructorFilePathParameterWithRealTypeTest :
        NetAspectTest<BeforeCallConstructorFilePathParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(null, MyAspect.FilePath);
                    var classToWeave_L = ClassToWeave.Create();
                    Assert.True(MyAspect.FilePath.EndsWith(@"FilePath\BeforeCallConstructorFilePathParameterWithRealTypeTest.cs"));
                };
        }

        public class ClassToWeave
        {
			[MyAspect]
            public ClassToWeave()
            {
            }

            public static ClassToWeave Create()
            {
                return new ClassToWeave();
            }
        }

        public class MyAspect : Attribute
        {
            public static string FilePath;
            public bool NetAspectAttribute = true;

            public void BeforeCallConstructor(string filePath)
            {
                FilePath = filePath;
            }
        }
    }
}