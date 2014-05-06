using System;
using NetAspect.Weaver.Core.Errors;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Getter.Parameters.After.Called
{
    public class AfterCallGetStaticFieldCalledParameterWithRealTypeTest :
        NetAspectTest<AfterCallGetStaticFieldCalledParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Warnings.Add(
                    string.Format(
                        "the called parameter in the method AfterGetField of the type '{0}' is not available for static field : default value will be passed",
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
            public static string Field = "Value";
            

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

                return ClassCalled.Field;
            }
        }

        public class MyAspect : Attribute
        {
            public static ClassCalled Called;
            public bool NetAspectAttribute = true;

            public void AfterGetField(ClassCalled called)
            {
                Called = called;
            }
        }
    }
}