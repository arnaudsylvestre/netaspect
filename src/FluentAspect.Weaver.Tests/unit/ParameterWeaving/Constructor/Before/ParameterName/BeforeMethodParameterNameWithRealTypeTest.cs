using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.Before.Instance
{
   public class BeforeConstructorParameterNameWithRealTypeTest :
        NetAspectTest<BeforeConstructorParameterNameWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                   Assert.IsNull(MyAspect.ParameterName);
                   var classToWeave_L = new ClassToWeave("value");
                    Assert.AreEqual("p", MyAspect.ParameterName);
                };
        }

        public class ClassToWeave
        {
            
            public ClassToWeave([MyAspect] string p)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static string ParameterName;
            public bool NetAspectAttribute = true;

            public void BeforeMethodForParameter(string parameterName)
            {
               ParameterName = parameterName;
            }
        }
    }
}