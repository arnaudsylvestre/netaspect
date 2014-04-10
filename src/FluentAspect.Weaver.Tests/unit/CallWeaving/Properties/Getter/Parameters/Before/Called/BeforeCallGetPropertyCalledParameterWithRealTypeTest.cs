using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Properties.Getter.Parameters.Before.Called
{
    public class BeforeCallGetPropertyCalledParameterWithRealTypeTest :
        NetAspectTest<BeforeCallGetPropertyCalledParameterWithRealTypeTest.ClassToWeave>
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
                };
        }

        public class ClassCalled
        {
            [MyAspect]
            public string Property = "Value";
            

        }

        public class ClassToWeave
        {
            ClassCalled called;

            public ClassToWeave(ClassCalled called)
            {
                this.called = called;
            }

            public string Weaved()
            {
                
                return called.Property;
            }
        }

        public class MyAspect : Attribute
        {
            public static ClassCalled Called;
            public bool NetAspectAttribute = true;

            public void BeforeGetProperty(ClassCalled called)
            {
                Called = called;
            }
        }
    }
}