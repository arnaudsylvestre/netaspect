using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.Before.CalledParameterName
{
    public class BeforeCallGetFieldCalledParameterNameParameterWithObjectTypeTest :
        NetAspectTest<BeforeCallGetFieldCalledParameterNameParameterWithObjectTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.ParameterName);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved(12);
                    Assert.AreEqual(12, MyAspect.ParameterName);
                };
        }

        public class ClassToWeave
        {
            [MyAspect] public string Field;

            public string Weaved(int param1)
            {
                return Field;
            }
        }

        public class MyAspect : Attribute
        {
            public static object ParameterName;
            public bool NetAspectAttribute = true;

            public void BeforeGetField(object calledParam1)
            {
                ParameterName = calledParam1;
            }
        }
    }
}