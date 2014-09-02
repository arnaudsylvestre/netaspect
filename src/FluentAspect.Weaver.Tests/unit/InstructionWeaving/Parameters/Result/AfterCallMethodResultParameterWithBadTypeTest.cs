using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.Result
{
    public class AfterCallMethodResultParameterWithBadTypeTest :
        NetAspectTest<AfterCallMethodResultParameterWithBadTypeTest.ClassToWeave>
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
                     "the result parameter in the method AfterCallMethod of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.String because the return type of the method Weaved in the type {1}",
                     typeof(MyAspect).FullName, typeof(ClassToWeave).FullName)
              });
       }

        public class ClassToWeave
        {
            [MyAspect]
            public string Method()
            {
                return "Hello";
            }

            public string Weaved()
            {
                return Method();
            }
        }

        public class MyAspect : Attribute
        {
            public static string Result;
            public bool NetAspectAttribute = true;

            public void AfterCallMethod(int result)
            {
               Result = result.ToString();
            }
        }
    }
}