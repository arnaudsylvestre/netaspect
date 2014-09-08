using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Update.Parameters.After.Property
{
   public class AfterCallUpdatePropertyPropertyParameterWithRealTypeTest :
      NetAspectTest<AfterCallUpdatePropertyPropertyParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(null, MyAspect.Property);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved();
            Assert.AreEqual("Property", MyAspect.Property.Name);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public string Property { get; set; }

         public void Weaved()
         {
            Property = "Dummy";
         }
      }

      public class MyAspect : Attribute
      {
         public static PropertyInfo Property;
         public bool NetAspectAttribute = true;

         public void AfterUpdateProperty(PropertyInfo property)
         {
            Property = property;
         }
      }
   }
}
