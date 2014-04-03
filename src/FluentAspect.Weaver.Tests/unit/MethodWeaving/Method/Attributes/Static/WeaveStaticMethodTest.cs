using System;
using System.Reflection;
using NUnit.Framework;
using NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Attributes.Visibility;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Attributes.Static
{
    public class WeaveStaticMethodTest : NetAspectTest<WeavePublicMethodTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Method);
                    ClassToWeave.Weaved();
                    Assert.AreEqual("Weaved", MyAspect.Method.Name);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public static void Weaved()
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static MethodInfo Method;
            public bool NetAspectAttribute = true;

            public void Before(MethodInfo method)
            {
                Method = method;
            }
        }
    }
}