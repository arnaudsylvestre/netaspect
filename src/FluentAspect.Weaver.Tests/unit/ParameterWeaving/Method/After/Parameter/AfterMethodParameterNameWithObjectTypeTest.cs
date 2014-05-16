using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Instance
{
    public class AfterMethodParameterWithObjectTypeTest :
        NetAspectTest<AfterMethodParameterWithObjectTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                   Assert.IsNull(MyAspect.Parameter);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved("value");
                    Assert.AreEqual("p", MyAspect.Parameter.Name);
                };
        }

        public class ClassToWeave
        {
            
            public void Weaved([MyAspect] string p)
            {
            }
        }

        public class MyAspect : Attribute
        {
           public static ParameterInfo Parameter;
            public bool NetAspectAttribute = true;

            public void AfterMethodForParameter(object parameter)
            {
               Parameter = (ParameterInfo) parameter;
            }
        }
    }
}