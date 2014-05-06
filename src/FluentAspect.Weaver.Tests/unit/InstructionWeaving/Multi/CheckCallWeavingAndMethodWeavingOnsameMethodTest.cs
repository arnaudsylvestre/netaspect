using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Multi
{
    public class CheckCallWeavingAndMethodWeavingOnsameMethodTest :
        NetAspectTest<CheckCallWeavingAndMethodWeavingOnsameMethodTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
            {
                Assert.IsNull(MyAspect.BeforeCalled);
                Assert.IsNull(MyAspect.BeforeGetFieldCalled);
                    ClassCalled called = new ClassCalled();
                    var classToWeave_L = new ClassToWeave(called);
                    classToWeave_L.Weaved();
                    Assert.AreEqual(classToWeave_L, MyAspect.BeforeCalled);
                    Assert.AreEqual(called, MyAspect.BeforeGetFieldCalled);
                };
        }

        public class ClassCalled
        {
            [MyAspect]
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
            public static ClassToWeave BeforeCalled;
            public static ClassCalled BeforeGetFieldCalled;
            public bool NetAspectAttribute = true;

            public void BeforeGetField(ClassCalled called)
            {
                BeforeGetFieldCalled = called;
            }
            public void Before(ClassToWeave instance)
            {
                BeforeCalled = instance;
            }
        }
    }
}