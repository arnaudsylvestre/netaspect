using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Update.Parameters.After.CalledParameterName
{
    public class AfterCallUpdateFieldCalledParameterNameReferencedParameterWithRealTypeReferencedTest :
        NetAspectTest<AfterCallUpdateFieldCalledParameterNameReferencedParameterWithRealTypeReferencedTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(0, MyAspect.ParameterName);
                    var classToWeave_L = new ClassToWeave();
                    int val = 12;
                    classToWeave_L.Weaved(ref val);
                    Assert.AreEqual(25, val);
                    Assert.AreEqual(12, MyAspect.ParameterName);
                };
        }

        public class ClassToWeave
        {
            [MyAspect] public string Field;

            public void Weaved(ref int param1)
            {
                Field = "Hello";
            }
        }

        public class MyAspect : Attribute
        {
            public static int ParameterName;
            public bool NetAspectAttribute = true;

            public void AfterUpdateField(ref int calledParam1)
            {
                ParameterName = calledParam1;
                calledParam1 = 25;
            }
        }
    }
}