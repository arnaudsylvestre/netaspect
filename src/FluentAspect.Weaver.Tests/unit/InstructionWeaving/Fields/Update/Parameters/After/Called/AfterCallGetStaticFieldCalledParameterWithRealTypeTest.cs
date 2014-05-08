using System;
using NetAspect.Weaver.Core.Errors;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Update.Parameters.After.Called
{
    public class AfterCallGetStaticFieldCalledParameterWithRealTypeTest :
        NetAspectTest<AfterCallGetStaticFieldCalledParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Warnings.Add(
                    string.Format(
                        "the called parameter in the method AfterUpdateField of the type '{0}' is not available for static field : default value will be passed",
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
            [MyAspect] public static string Field;


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

                ClassCalled.Field = "Dummy";
            }
        }

        public class MyAspect : Attribute
        {
            public static ClassCalled Called;
            public bool NetAspectAttribute = true;

            public void AfterUpdateField(ClassCalled called)
            {
                Called = called;
            }
        }
    }
}