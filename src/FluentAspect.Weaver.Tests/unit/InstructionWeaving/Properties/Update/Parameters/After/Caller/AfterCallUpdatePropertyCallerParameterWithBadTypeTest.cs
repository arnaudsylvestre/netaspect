using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Update.Parameters.After.Caller
{
    public class AfterCallUpdatePropertyCallerParameterWithBadTypeTest :
        NetAspectTest<AfterCallUpdatePropertyCallerParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the caller parameter in the method AfterSetProperty of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Object or {1}",
                        typeof (MyAspect).FullName,
                        typeof (ClassToWeave).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect] public string Property {get;set;}

            public void Weaved()
            {
                Property = "Dummy";
            }
        }

        public class MyAspect : Attribute
        {
            public static int Caller;
            public bool NetAspectAttribute = true;

            public void AfterSetProperty(int caller)
            {
                Caller = caller;
            }
        }
    }
}