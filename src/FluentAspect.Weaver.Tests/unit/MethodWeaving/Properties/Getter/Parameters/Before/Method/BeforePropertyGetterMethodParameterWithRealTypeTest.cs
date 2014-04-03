using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters.Before.Method
{
    public class BeforePropertyGetterMethodParameterWithRealTypeTest :
        NetAspectTest<BeforePropertyGetterMethodParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Method);
                    var classToWeave_L = new ClassToWeave();
                    string property = classToWeave_L.MyProperty;
                    Assert.AreEqual("get_MyProperty", MyAspect.Method.Name);
                };
        }


        public class ClassToWeave
        {
            [MyAspect]
            public string MyProperty
            {
                get { return ""; }
            }
        }

        public class MyAspect : Attribute
        {
            public static MethodInfo Method;
            public bool NetAspectAttribute = true;

            public void BeforePropertyGet(MethodInfo method)
            {
                Method = method;
            }
        }
    }
}