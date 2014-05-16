using System;
using System.Collections.Generic;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Instance
{
   public class AfterMethodParameterNameWithRealTypeReferencedTest :
        NetAspectTest<AfterMethodParameterNameWithRealTypeReferencedTest.ClassToWeave>
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
                    "impossible to ref/out the parameter 'parameterName' in the method AfterMethodForParameter of the type '{0}'",
                    typeof(MyAspect).FullName, typeof(string).FullName)
             });
      }

        public class ClassToWeave
        {
            
            public string Weaved([MyAspect] string parameter)
            {
               parameter = "OtherValue";
               return parameter;
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;

            public void AfterMethodForParameter(ref string parameterName)
            {
            }
        }
    }
}