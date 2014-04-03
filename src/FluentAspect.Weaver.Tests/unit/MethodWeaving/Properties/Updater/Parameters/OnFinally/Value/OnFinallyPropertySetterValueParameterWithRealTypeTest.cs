using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.OnFinally.Value
{
    public class OnFinallyPropertySetterValueParameterWithRealTypeTest :
        NetAspectTest<OnFinallyPropertySetterValueParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Value);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.MyProperty = "Value";
                    Assert.AreEqual("Value", MyAspect.Value);
                };
        }


        public class ClassToWeave
        {
            [MyAspect]
            public string MyProperty
            {
                set { }
            }
        }

        public class MyAspect : Attribute
        {
            public static string Value;
            public bool NetAspectAttribute = true;

            public void OnFinallyPropertySet(string value)
            {
                Value = value;
            }
        }
    }
}