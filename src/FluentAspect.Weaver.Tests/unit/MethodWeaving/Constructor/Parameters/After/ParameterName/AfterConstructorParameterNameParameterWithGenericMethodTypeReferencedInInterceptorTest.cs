using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After.ParameterName
{
    public class AfterConstructorParameterNameParameterWithGenericMethodTypeReferencedInInterceptorTest :
        NetAspectTest<AfterConstructorParameterNameParameterWithGenericMethodTypeReferencedInInterceptorTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return errorHandler => errorHandler.Add(new ErrorReport.Error()
                {
                    Level = ErrorLevel.Error,
                    Message = string.Format("Impossible to ref a generic parameter")
                });
        }

        public class ClassToWeave
        {
            [MyAspect]
            public void Weaved<T>(T i)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static object I;
            public bool NetAspectAttribute = true;

            public void AfterConstructor(ref object i)
            {
                I = i;
            }
        }
    }
}