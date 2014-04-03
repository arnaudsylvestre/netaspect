using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.Before.FilePath
{
    public class BeforeCallGetFieldFilePathParameterWithRealTypeTest :
        NetAspectTest<BeforeCallGetFieldFilePathParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(null, MyAspect.FilePath);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.AreEqual(@"D:\Developpement\fluentaspect\src\FluentAspect.Weaver.Tests\unit\CallWeaving\Fields\Getter\Parameters\Before\FilePath\BeforeCallGetFieldFilePathParameterWithRealTypeTest.cs", MyAspect.FilePath);
                };
        }

        public class ClassToWeave
        {
            [MyAspect] public string Field;

            public string Weaved()
            {
                return Field;
            }
        }

        public class MyAspect : Attribute
        {
            public static string FilePath;
            public bool NetAspectAttribute = true;

            public void BeforeGetField(string filePath)
            {
                FilePath = filePath;
            }
        }
    }
}