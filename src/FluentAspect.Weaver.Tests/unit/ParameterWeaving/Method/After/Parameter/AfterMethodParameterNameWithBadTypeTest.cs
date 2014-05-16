using System;
using System.Collections.Generic;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Instance
{
    public class AfterMethodParameterWithBadTypeTest :
        NetAspectTest<AfterMethodParameterWithBadTypeTest.ClassToWeave>
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
                     "the parameter parameter in the method AfterMethodForParameter of the type '{0}' is declared with the type 'System.Int32' but it is expected to be {1}",
                     typeof(MyAspect).FullName, typeof(ParameterInfo).FullName)
              });
       }

        public class ClassToWeave
        {
            
            public void Weaved([MyAspect] string parameter)
            {
               parameter = "OtherValue";
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void AfterMethodForParameter(int parameter)
            {
            }
        }
    }
}