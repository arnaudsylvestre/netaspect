using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.LifeCycle
{
    public class MethodWeavingWithNoLifeCycleTest :
        NetAspectTest<MethodWeavingWithNoLifeCycleTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
            {
                var classToWeave = new ClassToWeave();
                classToWeave.Weaved();
                Assert.AreEqual(1, MyAspect.aspects.Count);
                Assert.AreEqual(1, MyAspect.aspects[0].nbAfter);
                Assert.AreEqual(1, MyAspect.aspects[0].nbBefore);
            };
        }


        public class ClassToWeave
        {

            [MyAspect]
            public void Weaved()
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static List<MyAspect> aspects = new List<MyAspect>();
            public int nbAfter = 0;
            public int nbBefore = 0;
            public bool NetAspectAttribute = true;

            public MyAspect()
            {
                aspects.Add(this);
            }

            public void After()
            {
                nbAfter++;
            }

            public void Before()
            {
                nbBefore++;
            }
        }
    }
}