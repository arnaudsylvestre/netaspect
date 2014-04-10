using System;
using NUnit.Framework;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Properties.Getter.Parameters.After.Called
{
    public class AfterCallGetStaticPropertyCalledParameterWithRealTypeTest :
        NetAspectTest<AfterCallGetStaticPropertyCalledParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Warnings.Add(
                    string.Format(
                        "the called parameter in the method AfterGetProperty of the type '{0}' is not available for static property : default value will be passed",
                        typeof(MyAspect).FullName));
        }


        protected override Action CreateEnsure()
        {
            return () =>
            {
                Assert.IsNull(MyAspect.Called);
                ClassCalled called = new ClassCalled();
                var classToWeave_L = new ClassToWeave(called);
                classToWeave_L.Weaved();
                Assert.AreEqual(null, MyAspect.Called);
            };
        }

        public class ClassCalled
        {
            [MyAspect]
            public static string Property = "Value";
            

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

                return ClassCalled.Property;
            }
        }

        public class MyAspect : Attribute
        {
            public static ClassCalled Called;
            public bool NetAspectAttribute = true;

            public void AfterGetProperty(ClassCalled called)
            {
                Called = called;
            }
        }
    }
}