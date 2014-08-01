using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.LifeCycle
{
    public class InstructionWeavingChekSameInstanceTest :
        NetAspectTest<InstructionWeavingChekSameInstanceTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                   MyAspect.aspects = new List<MyAspect>();
                    Assert.AreEqual(0, MyAspect.aspects.Count);
                    ClassCalled called = new ClassCalled();
                    var classToWeave_L = new ClassToWeave(called);
                    classToWeave_L.Weaved();
                    var myAspect = MyAspect.aspects.First();
                    Assert.AreEqual(1, MyAspect.aspects.Count);
                    Assert.True(myAspect.after);
                    Assert.True(myAspect.before);
                };
        }

        public class ClassCalled
        {
            [MyAspect]
            public string Field = "Value";
            

        }

        public class ClassToWeave
        {
            ClassCalled called;

            public ClassToWeave(ClassCalled called)
            {
                this.called = called;
            }

            public string Weaved()
            {
                
                return called.Field;
            }
        }

        public class MyAspect : Attribute
        {
            public bool before = false;
            public bool after = false;

            public static List<MyAspect> aspects = new List<MyAspect>();

            public bool NetAspectAttribute = true;

            public MyAspect()
            {
                aspects.Add(this);
            }

            public void AfterGetField()
            {
                after = true;
            }
            public void BeforeGetField()
            {
                before = true;
            }
        }
    }
}