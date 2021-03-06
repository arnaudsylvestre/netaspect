using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.Value
{
   public class AfterPropertyValueParameterWithRealTypeByteReferencedInInterceptorTest :
      NetAspectTest<AfterPropertyValueParameterWithRealTypeByteReferencedInInterceptorTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.I);
            var classToWeave_L = new ClassToWeave();
            byte i = 12;
            classToWeave_L.Weaved = i;
            Assert.AreEqual(12, MyAspect.I);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public byte Weaved
         {
            set { }
         }
      }

      public class MyAspect : Attribute
      {
         public static int I;
         public bool NetAspectAttribute = true;

         public void AfterPropertySetMethod(ref byte propertyValue)
         {
             I = propertyValue;
         }
      }
   }
}
