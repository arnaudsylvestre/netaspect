using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.After.FilePath
{
    public class AfterCallGetFieldFilePathParameterWithRealTypeTest :
        NetAspectTest<AfterCallGetFieldFilePathParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(null, MyAspect.FilePath);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.AreEqual(@"D:\Developpement\fluentaspect\src\FluentAspect.Weaver.Tests\unit\CallWeaving\Fields\Getter\Parameters\After\FilePath\AfterCallGetFieldFilePathParameterWithRealTypeTest.cs", MyAspect.FilePath);
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

            public void AfterGetField(string filePath)
            {
                FilePath = filePath;
            }
        }
    }
}