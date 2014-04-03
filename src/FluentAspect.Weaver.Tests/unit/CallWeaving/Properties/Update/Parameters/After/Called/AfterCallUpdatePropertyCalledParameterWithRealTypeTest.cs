using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Properties.Update.Parameters.After.Called
{
    public class AfterCallUpdatePropertyCalledParameterWithRealTypeTest :
        NetAspectTest<AfterCallUpdatePropertyCalledParameterWithRealTypeTest.ClassToWeave>
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
            [MyAspect]
            public string Property { get; set; }

            public void Weaved()
            {
                Property = "Hello";
            }
        }

        public class MyAspect : Attribute
        {
            public static ClassToWeave Called;
            public bool NetAspectAttribute = true;

            public void AfterUpdateProperty(ClassToWeave called)
            {
                Called = called;
            }
        }
    }
}