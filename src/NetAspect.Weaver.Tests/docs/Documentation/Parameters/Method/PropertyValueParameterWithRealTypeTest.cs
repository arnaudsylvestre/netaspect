using System;
using NUnit.Framework;
using NetAspect.Weaver.Tests.unit;

namespace NetAspect.Weaver.Tests.docs.Documentation.Parameters.Method
{
   public class PropertyValueParameterWithRealTypeTest :
      NetAspectTest<PropertyValueParameterWithRealTypeTest.ClassToWeave>
   {
       public PropertyValueParameterWithRealTypeTest()
           : base("It must be declared with the same type as the property", "MethodWeavingBefore", "MethodWeaving")
       {
           
       }
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspectAttribute.I);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved = 12;
            Assert.AreEqual(12, MyAspectAttribute.I);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public int Weaved
         {
            set { }
         }
      }

      public class MyAspectAttribute : Attribute
      {
         public static int I;
         public bool NetAspectAttribute = true;

         public void AfterPropertySetMethod(int propertyValue)
         {
             I = propertyValue;
         }
      }
   }
}
