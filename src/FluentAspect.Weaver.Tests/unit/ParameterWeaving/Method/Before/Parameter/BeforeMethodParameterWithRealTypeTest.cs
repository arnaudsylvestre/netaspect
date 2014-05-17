using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.Before.Instance
{
   public class BeforeMethodParameterWithRealTypeTest :
        NetAspectTest<BeforeMethodParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                   Assert.IsNull(MyAspect.Parameter);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved(0, "value");
                    Assert.AreEqual("p", MyAspect.Parameter.Name);
                };
        }

        public class ClassToWeave
        {
            
            public void Weaved(int anotherParameter, [MyAspect] string p)
            {
               p = "";
            }
        }

        public class MyAspect : Attribute
        {
           public static ParameterInfo Parameter;
            public bool NetAspectAttribute = true;

            public void BeforeMethodForParameter(ParameterInfo parameter)
            {
               Parameter = parameter;
            }
        }
    }
}