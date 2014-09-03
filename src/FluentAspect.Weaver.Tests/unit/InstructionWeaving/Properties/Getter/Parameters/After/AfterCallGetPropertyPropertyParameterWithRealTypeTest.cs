using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Getter.Parameters.After.Field
{
   public class AfterCallGetPropertyPropertyParameterWithRealTypeTest :
      NetAspectTest<AfterCallGetPropertyPropertyParameterWithRealTypeTest.ClassToWeave>
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

         public string Weaved()
         {
            return Property;
         }
      }

      public class MyAspect : Attribute
      {
         public static PropertyInfo Property;
         public bool NetAspectAttribute = true;

         public void AfterGetProperty(PropertyInfo property)
         {
            Property = property;
         }
      }
   }
}
