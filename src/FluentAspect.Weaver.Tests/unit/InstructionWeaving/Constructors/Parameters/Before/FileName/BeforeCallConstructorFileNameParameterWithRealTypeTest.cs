using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Constructors.Parameters.Before.FileName
{
    public class BeforeCallConstructorFileNameParameterWithRealTypeTest :
        NetAspectTest<BeforeCallConstructorFileNameParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(null, MyAspect.FileName);
                    var classToWeave_L = ClassToWeave.Create();
                    Assert.AreEqual("BeforeCallConstructorFileNameParameterWithRealTypeTest.cs", MyAspect.FileName);
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
            public static string FileName;
            public bool NetAspectAttribute = true;

            public void BeforeCallConstructor(string fileName)
            {
                FileName = fileName;
            }
        }
    }
}