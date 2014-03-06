using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.Before.Called
{
    public class BeforeCallGetFieldCalledParameterWithRealTypeTest :
        NetAspectTest<BeforeCallGetFieldCalledParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Called);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.AreEqual(classToWeave_L, MyAspect.Called);
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
            public static ClassToWeave Called;
            public bool NetAspectAttribute = true;

            public void BeforeGetField(ClassToWeave called)
            {
                Called = called;
            }
        }
    }
}