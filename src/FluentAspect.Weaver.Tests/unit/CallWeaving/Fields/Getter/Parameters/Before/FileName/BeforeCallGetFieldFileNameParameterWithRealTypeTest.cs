using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.Before.FileName
{
    public class BeforeCallGetFieldFileNameParameterWithRealTypeTest :
        NetAspectTest<BeforeCallGetFieldFileNameParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(null, MyAspect.FileName);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.AreEqual("BeforeCallGetFieldFileNameParameterWithRealTypeTest.cs", MyAspect.FileName);
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
            public static string FileName;
            public bool NetAspectAttribute = true;

            public void BeforeGetField(string fileName)
            {
                FileName = fileName;
            }
        }
    }
}