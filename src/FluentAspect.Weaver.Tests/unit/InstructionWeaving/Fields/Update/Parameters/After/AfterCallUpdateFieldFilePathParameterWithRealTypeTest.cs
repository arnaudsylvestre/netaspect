using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Update.Parameters.After.FilePath
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
                    Assert.True(MyAspect.FilePath.EndsWith(@"After\AfterCallUpdateFieldFilePathParameterWithRealTypeTest.cs"));
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