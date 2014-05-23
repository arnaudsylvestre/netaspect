using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.Before.Instance
{
    public class BeforeMethodParameterValueWithBadTypeTest :
        NetAspectTest<BeforeMethodParameterValueWithBadTypeTest.ClassToWeave>
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
                     "the parameterValue parameter in the method BeforeMethodForParameter of the type '{0}' is declared with the type 'System.Int32' but it is expected to be {1}",
                     typeof(MyAspect).FullName, typeof(string).FullName)
              });
       }

        public class ClassToWeave
        {
            
            public void Weaved([MyAspect] string p)
            {
               p = "OtherValue";
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void BeforeMethodForParameter(int parameterValue)
            {
            }
        }
    }
}