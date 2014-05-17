using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Methods.Parameters.After.Called
{
    public class OverrideTest :
        NetAspectTest<OverrideTest.ClassToWeave>
    {
        [Test]
        public void Check()
        {
            A b = new B();
            A a2 = new A2();
            A a = new A();

            Assert.False(IsSameMethodAsA(b));
            Assert.True(IsSameMethodAsA(a));
            Assert.True(IsSameMethodAsA(a2));
        }

        private static bool IsSameMethodAsA(A a)
        {
            return "NetAspect.Weaver.Tests.unit.InstructionWeaving.Methods.Parameters.After.Called.OverrideTest+A" == a.GetType().GetMethod("Method").DeclaringType.FullName;
        }

        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.False(MyAspect.Passed);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved(new B());
                    Assert.False(MyAspect.Passed);
                    classToWeave_L.Weaved(new A());
                    Assert.True(MyAspect.Passed);
                };
        }


        public class A
        {
            [MyAspect]
            public virtual void Method()
            {

            }
        }

        public class A2 : A
        {
        }

        public class B : A2
        {
            public override void Method()
            {

            }
        }


        public class ClassToWeave
        {
            public void Weaved(A a)
            {
                a.Method();
            }
        }

        public class MyAspect : Attribute
        {
            public static bool Passed;
            public bool NetAspectAttribute = true;

            public void AfterCallMethod()
            {
                Passed = true;
            }
        }
    }
}