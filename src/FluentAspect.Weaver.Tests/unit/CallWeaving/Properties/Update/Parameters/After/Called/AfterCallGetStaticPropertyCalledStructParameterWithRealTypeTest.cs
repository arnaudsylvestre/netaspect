using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Properties.Update.Parameters.After.Called
{
    public class AfterCallGetStaticPropertyCalledStructParameterWithRealTypeTest :
        NetAspectTest<AfterCallGetStaticPropertyCalledStructParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the called parameter in the method AfterSetProperty of the type '{0}' is not available for static property in struct",
                        typeof(MyAspect).FullName));
        }

        public struct ClassCalled
        {
            [MyAspect]
            public static string Property {get;set;}
            

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

                ClassCalled.Property = "Dummy";
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