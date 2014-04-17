using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.Before.Method
{
    public class BeforePropertyPropertyInfoParameterWithRealTypeTest :
        NetAspectTest<BeforePropertyPropertyInfoParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.PropertyInfo);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved = "12";
                    Assert.AreEqual("Weaved", MyAspect.PropertyInfo.Name);
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

            public void BeforePropertySetMethod(PropertyInfo property)
            {
                PropertyInfo = property;
            }
        }
    }
}