using System;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Methods.Parameters.Before.CallerParameterName
{
    public class BeforeCallMethodCallerParameterNameParameterWithRealTypeOutTest : NetAspectTest<BeforeCallMethodCallerParameterNameParameterWithRealTypeOutTest.ClassToWeave>
    {
        protected override Action<FluentAspect.Weaver.Core.Errors.ErrorHandler> CreateErrorHandlerProvider()
        {
            return errorHandler => errorHandler.Errors.Add(string.Format("the instance parameter in the method Before of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Object or {1}", typeof(string).FullName, typeof(string).FullName));
        }

        public class ClassToWeave
        {

            [MyAspect]
            public string Method() {return "Hello";}

            public string Weaved(int param1)
            {
                return Method();
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void BeforeCallMethod(out int callerParam1)
            {
                callerParam1 = 12;
            }
        }
    }


}