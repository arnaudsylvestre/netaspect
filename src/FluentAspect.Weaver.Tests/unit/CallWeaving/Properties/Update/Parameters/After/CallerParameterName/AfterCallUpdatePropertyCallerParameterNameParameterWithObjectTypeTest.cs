using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.CallWeaving.Properties.Update.Parameters.After.CallerParameterName
{
    public class AfterCallUpdatePropertyCallerParameterNameParameterWithObjectTypeTest :
        NetAspectTest<AfterCallUpdatePropertyCallerParameterNameParameterWithObjectTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.AreEqual(null, MyAspect.ParameterName);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved(12);
                    Assert.AreEqual(12, MyAspect.ParameterName);
                };
        }

        public class ClassToWeave
        {
            [MyAspect] public string Property {get;set;}

            public void Weaved(int param1)
            {
                Property = "Dummy";
            }
        }

        public class MyAspect : Attribute
        {
            public static object ParameterName;
            public bool NetAspectAttribute = true;

            public void AfterUpdateProperty(object callerParam1)
            {
                ParameterName = callerParam1;
            }
        }
    }
}