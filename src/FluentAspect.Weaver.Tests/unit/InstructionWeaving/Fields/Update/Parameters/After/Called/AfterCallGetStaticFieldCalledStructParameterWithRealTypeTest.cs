using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Update.Parameters.After.Called
{
    public class AfterCallGetStaticFieldCalledStructParameterWithRealTypeTest :
        NetAspectTest<AfterCallGetStaticFieldCalledStructParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the called parameter in the method AfterUpdateField of the type '{0}' is not available for static field in struct",
                        typeof(MyAspect).FullName));
        }

        public struct ClassCalled
        {
            [MyAspect]
            public static string Field;
            

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