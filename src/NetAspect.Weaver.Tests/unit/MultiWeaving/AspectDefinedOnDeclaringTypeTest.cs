using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MultiWeaving
{
   public class AspectDefinedOnDeclaringTypeTest :
      NetAspectTest<AspectDefinedOnDeclaringTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.Instance);
            var instance = new ClassCalled();
            var classToWeave_L = new ClassToWeave(instance);
            classToWeave_L.Weaved();
            Assert.AreEqual(instance, MyAspect.Instance);
         };
      }

      [MyAspect]
      public class ClassCalled
      {
         public string Field = "Value";
      }

      public class ClassToWeave
      {
         private readonly ClassCalled _instance;

         public ClassToWeave(ClassCalled instance)
         {
            this._instance = instance;
         }

         public string Weaved()
         {
            return _instance.Field;
         }
      }

      public class MyAspect : Attribute
      {
         public static ClassCalled Instance;
         public bool NetAspectAttribute = true;

         public void AfterGetField(ClassCalled instance)
         {
            Instance = instance;
         }
      }
   }
}
