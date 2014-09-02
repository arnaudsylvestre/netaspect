using System;
using System.Collections.Generic;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.ParameterWeaving.Parameters.Parameter
{
    public class AfterConstructorParameterWithBadTypeTest :
        NetAspectTest<AfterConstructorParameterWithBadTypeTest.ClassToWeave>
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
                     "the parameter parameter in the method AfterConstructorForParameter of the type '{0}' is declared with the type 'System.Int32' but it is expected to be {1}",
                     typeof(MyAspect).FullName, typeof(ParameterInfo).FullName)
              });
       }

        public class ClassToWeave
        {
            
            public ClassToWeave([MyAspect] string p)
            {
               p = "OtherValue";
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void AfterConstructorForParameter(int parameter)
            {
            }
        }
    }
}