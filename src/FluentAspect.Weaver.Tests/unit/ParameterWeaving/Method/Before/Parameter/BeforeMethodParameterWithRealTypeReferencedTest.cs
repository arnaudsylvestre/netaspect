using System;
using System.Collections.Generic;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.Before.Instance
{
   public class BeforeMethodParameterWithRealTypeReferencedTest :
        NetAspectTest<BeforeMethodParameterWithRealTypeReferencedTest.ClassToWeave>
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
                    "impossible to ref/out the parameter 'parameter' in the method BeforeMethodForParameter of the type '{0}'",
                    typeof(MyAspect).FullName)
             });
      }

        public class ClassToWeave
        {
            
            public string Weaved([MyAspect] string p)
            {
               p = "OtherValue";
               return p;
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void BeforeMethodForParameter(ref ParameterInfo parameter)
            {
            }
        }
    }
}