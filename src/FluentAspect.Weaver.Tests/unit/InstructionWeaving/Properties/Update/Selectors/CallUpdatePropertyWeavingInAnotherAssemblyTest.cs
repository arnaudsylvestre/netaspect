using System;
using System.Collections.Generic;
using System.Reflection;
using NetAspect.Sample.Dep;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Update.Selectors
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

            public void BeforeSetProperty(DepClassWhichCallProperty caller)
            {
                Caller = caller;
            }

            public static bool SelectProperty(PropertyInfo property)
            {
                return property.Name == "Property";
            }
        }
    }
}