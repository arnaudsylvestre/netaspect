using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Getter.Selectors.Infos
{
   public class GetPropertyWithSelectorPropertyAllParametersTest :
      NetAspectTest<GetPropertyWithSelectorPropertyAllParametersTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.Caller);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved();
            Assert.AreEqual(classToWeave_L, MyAspect.Caller);
         };
      }

      public class ClassToWeave
      {
         public string Property { get; set; }

         public string Weaved()
         {
            return Property;
         }
      }

      public class MyAspect : Attribute
      {
         public static ClassToWeave Caller;
         public bool NetAspectAttribute = true;

         public void BeforeGetProperty(ClassToWeave caller)
         {
            Caller = caller;
         }

         public static bool SelectProperty(PropertyInfo property)
         {
            return property.Name == "Property";
         }
      }
   }
}
