using System;
using System.Collections.Generic;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.Constructor
{
    public class AfterConstructorConstructorInfoParameterWithRealTypeReferencedTest :
        NetAspectTest<AfterConstructorConstructorInfoParameterWithRealTypeReferencedTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Add(new ErrorReport.Error()
                {
                    Level = ErrorLevel.Error,
                    Message =
                    string.Format("impossible to ref/out the parameter 'constructor' in the method AfterConstructor of the type '{0}'",
                                  typeof(MyAspect).FullName)
                });
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave()
            {
            }
        }

        public class MyAspect : Attribute
        {
           public static MethodBase Method;
            public bool NetAspectAttribute = true;

            public void AfterConstructor(ref MethodBase constructor)
            {
                Method = constructor;
            }
        }
    }
}