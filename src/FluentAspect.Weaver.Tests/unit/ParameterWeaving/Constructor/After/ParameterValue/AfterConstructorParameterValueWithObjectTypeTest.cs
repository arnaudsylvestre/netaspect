using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Instance
{
   public class AfterConstructorParameterValueWithObjectTypeTest :
        NetAspectTest<AfterConstructorParameterValueWithObjectTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                   Assert.IsNull(MyAspect.ParameterValue);
                   var classToWeave_L = new ClassToWeave("value");
                    
                    Assert.AreEqual("OtherValue", MyAspect.ParameterValue);
                };
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
           public static object ParameterValue;
            public bool NetAspectAttribute = true;

            public void AfterConstructorForParameter(object parameterValue)
            {
               ParameterValue = parameterValue;
            }
        }
    }
}