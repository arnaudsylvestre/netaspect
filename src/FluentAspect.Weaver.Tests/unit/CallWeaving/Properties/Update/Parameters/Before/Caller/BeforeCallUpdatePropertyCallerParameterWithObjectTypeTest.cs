using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Properties.Update.Parameters.Before.Caller
{
    public class BeforeCallUpdatePropertyCallerParameterWithObjectTypeTest :
        NetAspectTest<BeforeCallUpdatePropertyCallerParameterWithObjectTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Caller);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.AreEqual(classToWeave_L, MyAspect.Caller);
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
            public static object Caller;
            public bool NetAspectAttribute = true;

            public void BeforeUpdateProperty(object caller)
            {
                Caller = caller;
            }
        }
    }
}