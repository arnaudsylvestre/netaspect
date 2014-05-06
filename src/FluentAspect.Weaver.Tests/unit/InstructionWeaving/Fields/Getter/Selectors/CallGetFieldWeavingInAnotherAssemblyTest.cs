using System;
using System.Collections.Generic;
using System.Reflection;
using NetAspect.Sample.Dep;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Getter.Selectors
{
    public class CallGetFieldWeavingInAnotherAssemblyTest :
        NetAspectTest<CallGetFieldWeavingInAnotherAssemblyTest.MyAspect, DepClassWhichCallField>
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

            public void BeforeGetField(DepClassWhichCallField caller)
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