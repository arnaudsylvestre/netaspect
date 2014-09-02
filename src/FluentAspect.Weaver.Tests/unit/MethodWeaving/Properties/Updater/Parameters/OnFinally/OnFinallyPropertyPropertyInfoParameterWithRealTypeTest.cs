using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.OnFinally.Property
{
    public class OnFinallyPropertyPropertyInfoParameterWithRealTypeTest :
        NetAspectTest<OnFinallyPropertyPropertyInfoParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.PropertyInfo);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved = "12";
                    Assert.AreEqual(classToWeave_L.GetType().GetProperty("Weaved"), MyAspect.PropertyInfo);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public string Weaved
           {
              set { }
            }
        }

        public class MyAspect : Attribute
        {
            public static PropertyInfo PropertyInfo;
            public bool NetAspectAttribute = true;

            public void OnFinallyPropertySetMethod(PropertyInfo Property)
            {
                PropertyInfo = Property;
            }
        }
    }
}