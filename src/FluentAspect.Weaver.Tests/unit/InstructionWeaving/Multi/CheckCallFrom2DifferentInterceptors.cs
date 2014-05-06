using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Multi
{
    public class CheckCallFrom2DifferentInterceptors :
        NetAspectTest<CheckCallFrom2DifferentInterceptors.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
            {
                Assert.IsNull(MyAspect2.BeforeGetFieldCalled);
                Assert.IsNull(MyAspect.BeforeGetFieldCalled);
                    ClassCalled called = new ClassCalled();
                    var classToWeave_L = new ClassToWeave(called);
                    classToWeave_L.Weaved();
                    Assert.AreEqual(called, MyAspect.BeforeGetFieldCalled);
                    Assert.AreEqual(called, MyAspect2.BeforeGetFieldCalled);
                };
        }

        public class ClassCalled
        {
            [MyAspect]
            [MyAspect2]
            public string Field = "Value";
            

        }

        public class ClassToWeave
        {
            ClassCalled called;

            public ClassToWeave(ClassCalled called)
            {
                this.called = called;
            }

            [MyAspect]
            public string Weaved()
            {
                return called.Field;
            }
        }

        public class MyAspect : Attribute
        {
            public static ClassCalled BeforeGetFieldCalled;
            public bool NetAspectAttribute = true;

            public void BeforeGetField(ClassCalled called)
            {
                BeforeGetFieldCalled = called;
            }
        }
        public class MyAspect2 : Attribute
        {
            public static ClassCalled BeforeGetFieldCalled;
            public bool NetAspectAttribute = true;

            public void BeforeGetField(ClassCalled called)
            {
                BeforeGetFieldCalled = called;
            }
        }
    }
}