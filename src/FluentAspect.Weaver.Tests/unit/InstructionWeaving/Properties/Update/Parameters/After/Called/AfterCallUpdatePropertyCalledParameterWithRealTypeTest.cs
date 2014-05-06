using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Update.Parameters.After.Called
{
    public class AfterCallUpdatePropertyCalledParameterWithRealTypeTest :
        NetAspectTest<AfterCallUpdatePropertyCalledParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Called);
                    ClassCalled called = new ClassCalled();
                    var classToWeave_L = new ClassToWeave(called);
                    classToWeave_L.Weaved();
                    Assert.AreEqual(called, MyAspect.Called);
                    Assert.AreEqual("Dummy", called.Property);
                };
        }

        public class ClassCalled
        {
           [MyAspect]
           public string Property { get; set; }
        }

        public class ClassToWeave
        {
            ClassCalled called;

            public ClassToWeave(ClassCalled called)
            {
                this.called = called;
            }

            public void Weaved()
            {
                called.Property = "Dummy";
            }
        }

        public class MyAspect : Attribute
        {
            public static ClassCalled Called;
            public bool NetAspectAttribute = true;

            public void AfterSetProperty(ClassCalled called)
            {
                Called = called;
            }
        }
    }
}