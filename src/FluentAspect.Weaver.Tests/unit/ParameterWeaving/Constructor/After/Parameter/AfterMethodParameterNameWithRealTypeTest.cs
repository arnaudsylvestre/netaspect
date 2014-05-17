using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Instance
{
   public class AfterConstructorParameterWithRealTypeTest :
        NetAspectTest<AfterConstructorParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                   Assert.IsNull(MyAspect.Parameter);
                    var classToWeave_L = new ClassToWeave(0, "value");
                    
                    Assert.AreEqual("p", MyAspect.Parameter.Name);
                };
        }

        public class ClassToWeave
        {
            
            public ClassToWeave(int anotherParameter, [MyAspect] string p)
            {
               p = "";
            }
        }

        public class MyAspect : Attribute
        {
           public static ParameterInfo Parameter;
            public bool NetAspectAttribute = true;

            public void AfterMethodForParameter(ParameterInfo parameter)
            {
               Parameter = parameter;
            }
        }
    }
}