using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Update.Parameters.Before.Called
{
    public class BeforeCallUpdateFieldCalledParameterWithRealTypeTest :
        NetAspectTest<BeforeCallUpdateFieldCalledParameterWithRealTypeTest.ClassToWeave>
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

            public void Weaved()
            {
                Field = "Hello";
            }
        }

        public class MyAspect : Attribute
        {
            public static ClassToWeave Called;
            public bool NetAspectAttribute = true;

            public void BeforeUpdateField(ClassToWeave called)
            {
                Called = called;
            }
        }
    }
}