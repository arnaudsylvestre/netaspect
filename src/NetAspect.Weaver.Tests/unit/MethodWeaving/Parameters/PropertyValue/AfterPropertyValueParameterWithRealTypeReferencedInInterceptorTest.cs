using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.Value
{
   public class AfterPropertyValueParameterWithRealTypeReferencedInInterceptorTest :
      NetAspectTest<AfterPropertyValueParameterWithRealTypeReferencedInInterceptorTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.I);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved = 12;
            Assert.AreEqual(12, classToWeave_L.I);
            Assert.AreEqual(12, MyAspect.I);
         };
      }

      public class ClassToWeave
      {
         public int I;

         [MyAspect]
         public int Weaved
         {
            set { I = value; }
         }
      }

      public class MyAspect : Attribute
      {
         public static int I;
         public bool NetAspectAttribute = true;

         public void AfterPropertySetMethod(ref int propertyValue)
         {
             I = propertyValue;
             propertyValue = 3;
         }
      }
   }
}
