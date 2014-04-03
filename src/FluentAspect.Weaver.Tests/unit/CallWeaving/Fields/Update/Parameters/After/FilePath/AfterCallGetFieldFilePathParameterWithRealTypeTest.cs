using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Fields.Update.Parameters.After.FilePath
{
    public class AfterCallUpdateFieldFilePathParameterWithRealTypeTest :
        NetAspectTest<AfterCallUpdateFieldFilePathParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(null, MyAspect.FilePath);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.AreEqual(@"D:\Developpement\fluentaspect\src\FluentAspect.Weaver.Tests\unit\CallWeaving\Fields\Updater\Parameters\After\FilePath\AfterCallUpdateFieldFilePathParameterWithRealTypeTest.cs", MyAspect.FilePath);
                };
        }

        public class ClassToWeave
        {
            [MyAspect] public string Field;

            public void Weaved()
            {
                Field = "Dummy";
            }
        }

        public class MyAspect : Attribute
        {
            public static string FilePath;
            public bool NetAspectAttribute = true;

            public void AfterUpdateField(string filePath)
            {
                FilePath = filePath;
            }
        }
    }
}