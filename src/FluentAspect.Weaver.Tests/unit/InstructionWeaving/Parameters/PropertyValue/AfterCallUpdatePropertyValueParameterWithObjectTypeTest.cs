using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.PropertyValue
{
   public class AfterCallUpdatePropertyValueParameterWithObjectTypeTest :
      NetAspectTest<AfterCallUpdatePropertyValueParameterWithObjectTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(null, MyAspect.Value);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved();
            Assert.AreEqual("Hello", MyAspect.Value);
         };
      }


      public class ClassToWeave
      {
          [MyAspect]
          public string Property { get; set; }

         public string Weaved()
         {
            Property = "Hello";
            return Property;
         }
      }

      public class MyAspect : Attribute
      {
         public static string Value;
         public bool NetAspectAttribute = true;

         public void AfterUpdateProperty(object propertyValue)
         {
             Value = propertyValue.ToString();
         }
      }
   }
}
