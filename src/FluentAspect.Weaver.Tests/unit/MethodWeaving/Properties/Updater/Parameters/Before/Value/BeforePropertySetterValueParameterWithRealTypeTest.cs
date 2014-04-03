using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.Before.Value
{
    public class BeforePropertySetterValueParameterWithRealTypeTest :
        NetAspectTest<BeforePropertySetterValueParameterWithRealTypeTest.ClassToWeave>
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

            public void BeforePropertySet(string value)
            {
                Value = value;
            }
        }
    }
}