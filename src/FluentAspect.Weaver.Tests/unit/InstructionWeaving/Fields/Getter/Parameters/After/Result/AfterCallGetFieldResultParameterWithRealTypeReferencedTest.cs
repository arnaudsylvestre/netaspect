using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Getter.Parameters.After.Result
{
    public class AfterCallGetFieldResultParameterWithRealTypeReferencedTest :
        NetAspectTest<AfterCallGetFieldResultParameterWithRealTypeReferencedTest.ClassToWeave>
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
                     "impossible to ref/out the parameter 'result' in the method AfterGetField of the type '{0}'",
                     typeof(MyAspect).FullName)
              });
       }

       public class ClassToWeave
       {
          [MyAspect]
          public string field = "Hello";

          public string Weaved()
          {
             return field;
          }
       }

        public class MyAspect : Attribute
        {
            public static string Result;
            public bool NetAspectAttribute = true;

            public void AfterGetField(ref string result)
            {
               Result = result;
               result = "New Hello";
            }
        }
    }
}