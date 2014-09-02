using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.ParameterWeaving.Parameters.ParameterName
{
   public class AfterConstructorParameterNameWithRealTypeReferencedTest :
        NetAspectTest<AfterConstructorParameterNameWithRealTypeReferencedTest.ClassToWeave>
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
                    "impossible to ref/out the parameter 'parameterName' in the method AfterConstructorForParameter of the type '{0}'",
                    typeof(MyAspect).FullName)
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

            public void AfterConstructorForParameter(ref string parameterName)
            {
            }
        }
    }
}