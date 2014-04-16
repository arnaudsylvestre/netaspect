using System;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.After.Errors
{
    public class AfterPropertyParameterNameSameAsKeywordTest :
        NetAspectTest<AfterPropertyParameterNameSameAsKeywordTest.ClassToWeave>
    {
        protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        {
            return errorHandler =>
                {
                    errorHandler.Errors.Add(string.Format("The parameter instance is already declared"));
                    errorHandler.Errors.Add(
                        string.Format(
                            "the instance parameter in the Property After of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.Object or {1}", typeof(MyAspect), typeof(ClassToWeave)));
                };
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

            public void After(int instance)
            {
            }
        }
    }
}