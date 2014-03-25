using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Updater.Parameters.After.FileName
{
    public class AfterCallUpdateFieldFileNameParameterWithRealTypeTest :
        NetAspectTest<AfterCallUpdateFieldFileNameParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(null, MyAspect.FileName);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.AreEqual("AfterCallUpdateFieldFileNameParameterWithRealTypeTest.cs", MyAspect.FileName);
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
            public static string FileName;
            public bool NetAspectAttribute = true;

            public void AfterUpdateField(string fileName)
            {
                FileName = fileName;
            }
        }
    }
}