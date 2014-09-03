using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.ParameterName
{
   public class AfterMethodParameterNameParameterWithRealTypeSByteReferencedInInterceptorTest :
      NetAspectTest<AfterMethodParameterNameParameterWithRealTypeSByteReferencedInInterceptorTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.AreEqual(0, MyAspect.I);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved(12);
            Assert.AreEqual(12, MyAspect.I);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public void Weaved(sbyte i)
         {
         }
      }

      public class MyAspect : Attribute
      {
         public static sbyte I;
         public bool NetAspectAttribute = true;

         public void After(ref sbyte i)
         {
            I = i;
         }
      }
   }
}
