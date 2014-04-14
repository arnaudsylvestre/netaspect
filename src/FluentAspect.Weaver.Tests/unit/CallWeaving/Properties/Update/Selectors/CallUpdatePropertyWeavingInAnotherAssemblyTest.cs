using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using NetAspect.Sample.Dep;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Properties.Update.Selectors
{
    public class CallUpdatePropertyWeavingInAnotherAssemblyTest :
        NetAspectTest<CallUpdatePropertyWeavingInAnotherAssemblyTest.MyAspect, DepClassWhichCallProperty>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Caller);
                    var classToWeave_L = new DepClassWhichCallProperty();
                    classToWeave_L.CallProperty("value");
                    Assert.AreEqual(classToWeave_L, MyAspect.Caller);
                };
        }

        public class MyAspect : Attribute
        {
            public static DepClassWhichCallProperty Caller;
            public bool NetAspectAttribute = true;


            public static IEnumerable<Assembly> AssembliesToWeave = new List<Assembly> { typeof(DepClassWhichCallProperty).Assembly };

            public void BeforeUpdateProperty(DepClassWhichCallProperty caller)
            {
                Caller = caller;
            }

            public static bool SelectProperty(string PropertyName)
            {
                return PropertyName == "Property";
            }
        }
    }
}