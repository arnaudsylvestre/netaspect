using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Update.Parameters.After.FileName
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
                    Assert.AreEqual("", MyAspect.FileName);
                };
        }

        public class ClassToWeave
        {
            [MyAspect] public string Field;

            public void Weaved()
            {
                Field = "Hello";
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