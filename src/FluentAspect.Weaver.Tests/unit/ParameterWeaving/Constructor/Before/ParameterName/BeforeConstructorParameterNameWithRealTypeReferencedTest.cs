using System;
using System.Collections.Generic;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.Before.Instance
{
   public class BeforeConstructorParameterNameWithRealTypeReferencedTest :
        NetAspectTest<BeforeConstructorParameterNameWithRealTypeReferencedTest.ClassToWeave>
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
                    "impossible to ref/out the parameter 'parameterName' in the method BeforeConstructorForParameter of the type '{0}'",
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

            public void BeforeConstructorForParameter(ref string parameterName)
            {
            }
        }
    }
}