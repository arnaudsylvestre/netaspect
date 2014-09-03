using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Attributes.Visibility
{
   public class WeaveInternalPropertyTest : NetAspectTest<WeaveInternalPropertyTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.Instance);
            var classToWeave_L = new ClassToWeave();
            string value = classToWeave_L.Weaved;
            Assert.AreEqual(classToWeave_L, MyAspect.Instance);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         internal string Weaved
         {
            get { return "12"; }
         }
      }

      public class MyAspect : Attribute
      {
         public static ClassToWeave Instance;
         public bool NetAspectAttribute = true;

         public void BeforePropertyGetMethod(ClassToWeave instance)
         {
            Instance = instance;
         }
      }
   }
}
