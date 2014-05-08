using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.OnException.Property
{
    public class OnExceptionPropertyPropertyInfoParameterWithBadTypeTest :
        NetAspectTest<OnExceptionPropertyPropertyInfoParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Errors.Add(
                    string.Format(
                        "the Property parameter in the method OnExceptionPropertySetMethod of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Reflection.PropertyInfo",
                        typeof (MyAspect).FullName));
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Weaved
           {
              set { }
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void OnExceptionPropertySetMethod(int Property)
            {
            }
        }
    }
}