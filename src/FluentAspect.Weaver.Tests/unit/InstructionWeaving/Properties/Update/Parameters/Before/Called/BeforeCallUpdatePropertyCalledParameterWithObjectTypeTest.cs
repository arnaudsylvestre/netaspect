using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Update.Parameters.Before.Called
{
    public class BeforeCallUpdatePropertyCalledParameterWithObjectTypeTest :
        NetAspectTest<BeforeCallUpdatePropertyCalledParameterWithObjectTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Called);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved();
                    Assert.AreEqual(classToWeave_L, MyAspect.Called);
                };
        }

        public class ClassToWeave
        {
            [MyAspect] public string Property {get;set;}

            public void Weaved()
            {
                Property = "Dummy";
            }
        }

        public class MyAspect : Attribute
        {
            public static object Called;
            public bool NetAspectAttribute = true;

            public void BeforeSetProperty(object called)
            {
                Called = called;
            }
        }
    }
}