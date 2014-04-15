using System;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters.OnFinally.Property
{
    public class OnFinallyPropertyPropertyInfoParameterWithRealTypeReferencedTest :
        NetAspectTest<OnFinallyPropertyPropertyInfoParameterWithRealTypeReferencedTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "impossible to ref/out the parameter 'property' in the property OnFinally of the type '{0}'",
                        typeof (MyAspect).FullName));
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
            public static PropertyInfo Property;
            public bool NetAspectAttribute = true;

            public void OnFinally(ref PropertyInfo property)
            {
                Property = property;
            }
        }
    }
}