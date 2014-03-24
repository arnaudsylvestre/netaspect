using System;
using FluentAspect.Sample.Dep;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.AnotherAssembly
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

            public void BeforeGetField(DepClassWhichCallField caller)
            {
                Caller = caller;
            }
        }
    }
}