using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.Before.Instance
{
    public class BeforeMethodParameterWithObjectTypeTest :
        NetAspectTest<BeforeMethodParameterWithObjectTypeTest.ClassToWeave>
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

            public void BeforeMethodForParameter(object parameter)
            {
               Parameter = (ParameterInfo) parameter;
            }
        }
    }
}