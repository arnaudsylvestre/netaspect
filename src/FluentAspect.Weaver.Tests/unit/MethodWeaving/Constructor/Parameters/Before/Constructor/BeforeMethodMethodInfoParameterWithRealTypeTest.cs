using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.Before.Method
{
    public class BeforeConstructorConstructorParameterWithRealTypeTest :
        NetAspectTest<BeforeConstructorConstructorParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Constructor);
                    var classToWeave_L = new ClassToWeave();
                    Assert.AreEqual(classToWeave_L.GetType().GetMethod("Weaved"), MyAspect.Constructor);
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave()
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static ConstructorInfo Constructor;
            public bool NetAspectAttribute = true;

            public void BeforeConstructor(ConstructorInfo constructor)
            {
                Constructor = constructor;
            }
        }
    }
}