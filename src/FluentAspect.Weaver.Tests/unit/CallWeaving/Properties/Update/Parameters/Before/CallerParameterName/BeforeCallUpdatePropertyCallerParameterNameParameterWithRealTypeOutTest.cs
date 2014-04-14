using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Properties.Update.Parameters.Before.CallerParameterName
{
    public class BeforeCallUpdatePropertyCallerParameterNameParameterWithRealTypeOutTest :
        NetAspectTest<BeforeCallUpdatePropertyCallerParameterNameParameterWithRealTypeOutTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to out the parameter 'callerParam1' in the method BeforeUpdateProperty of the type '{0}'",
                        typeof (MyAspect).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect] public string Property {get;set;}

            public void Weaved(int param1)
            {
                Property = "Dummy";
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void BeforeUpdateProperty(out int callerParam1)
            {
                callerParam1 = 12;
            }
        }
    }
}