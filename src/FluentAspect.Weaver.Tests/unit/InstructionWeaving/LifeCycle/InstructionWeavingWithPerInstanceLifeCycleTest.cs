using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.LifeCycle
{
   public class InstructionWeavingWithPerInstanceLifeCycleTest :
      NetAspectTest<InstructionWeavingWithPerInstanceLifeCycleTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            MyAspect.aspects = new List<MyAspect>();
            var called = new ClassCalled();
            var classToWeave_L = new ClassToWeave(called);
            classToWeave_L.Weaved();
            MyAspect aspect1 = MyAspect.aspects.First();
            Assert.AreEqual(1, aspect1.i);
            classToWeave_L.Weaved();
            Assert.AreEqual(2, aspect1.i);
            Assert.AreEqual(1, MyAspect.aspects.Count);
            var classToWeave2_L = new ClassToWeave(called);
            classToWeave2_L.Weaved();
            Assert.AreEqual(2, MyAspect.aspects.Count);
            Assert.AreEqual(2, aspect1.i);
            Assert.AreEqual(1, MyAspect.aspects.First(pair => pair != aspect1).i);
         };
      }

      public class ClassCalled
      {
         [MyAspect] public string Field = "Value";
      }

      public class ClassToWeave
      {
         private readonly ClassCalled called;

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
         public static List<MyAspect> aspects = new List<MyAspect>();

         public static string LifeCycle = "PerInstance";
         public bool NetAspectAttribute = true;
         public int i = 0;

         static MyAspect()
         {
            aspects = new List<MyAspect>();
         }

         public MyAspect()
         {
            aspects.Add(this);
         }

         public void AfterGetField()
         {
            i++;
         }
      }
   }
}
