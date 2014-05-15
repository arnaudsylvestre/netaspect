using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.LifeCycle
{
    public class MethodWeavingWithPerTypeLifeCycleTest :
        NetAspectTest<MethodWeavingWithPerTypeLifeCycleTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    var classToWeave = new ClassToWeave();
                    classToWeave.Weaved();
                    classToWeave = new ClassToWeave();
                    classToWeave.Weaved();
                    Assert.AreEqual(1, MyAspect.aspects.Count);
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

            public static string LifeCycle = "PerType";

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