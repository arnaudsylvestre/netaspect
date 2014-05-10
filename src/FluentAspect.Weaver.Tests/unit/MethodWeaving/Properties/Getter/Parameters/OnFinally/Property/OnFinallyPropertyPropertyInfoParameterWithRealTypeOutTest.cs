using System;
using System.Collections.Generic;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters.OnFinally.Property
{
    public class OnFinallyPropertyPropertyInfoParameterWithRealTypeOutTest :
        NetAspectTest<OnFinallyPropertyPropertyInfoParameterWithRealTypeOutTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Add(new ErrorReport.Error()
                {
                    Level = ErrorLevel.Error,
                    Message =
                    string.Format(
                        "impossible to ref/out the parameter 'property' in the method OnFinallyPropertyGetMethod of the type '{0}'",
                        typeof(MyAspect).FullName)
                });
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Weaved
            {
                get { return "12"; }
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void OnFinallyPropertyGetMethod(out PropertyInfo property)
            {
                property = null;
            }
        }
    }
}