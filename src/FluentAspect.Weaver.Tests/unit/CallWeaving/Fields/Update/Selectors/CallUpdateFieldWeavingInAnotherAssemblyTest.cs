using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using NetAspect.Sample.Dep;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Fields.Update.Selectors
{
    public class CallUpdateFieldWeavingInAnotherAssemblyTest :
        NetAspectTest<CallUpdateFieldWeavingInAnotherAssemblyTest.MyAspect, DepClassWhichCallField>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Caller);
                    var classToWeave_L = new DepClassWhichCallField();
                    classToWeave_L.CallField("value");
                    Assert.AreEqual(classToWeave_L, MyAspect.Caller);
                };
        }

        public class MyAspect : Attribute
        {
            public static DepClassWhichCallField Caller;
            public bool NetAspectAttribute = true;


            public static IEnumerable<Assembly> AssembliesToWeave = new List<Assembly> { typeof(DepClassWhichCallField).Assembly };

            public void BeforeUpdateField(DepClassWhichCallField caller)
            {
                Caller = caller;
            }

            public static bool SelectField(string fieldName)
            {
                return fieldName == "Field";
            }
        }
    }
}