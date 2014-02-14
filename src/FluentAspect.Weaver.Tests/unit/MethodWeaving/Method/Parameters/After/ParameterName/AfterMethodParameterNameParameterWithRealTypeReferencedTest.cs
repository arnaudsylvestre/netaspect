using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.ParameterName
{
   public class AfterMethodParameterNameParameterWithRealTypeReferencedTest : NetAspectTest<AfterMethodParameterNameParameterWithRealTypeReferencedTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.AreEqual(0, MyAspect.I);
               var classToWeave_L = new ClassToWeave();
               int value = 12;
               Assert.AreEqual(8, classToWeave_L.Weaved(ref value));
               Assert.AreEqual(8, MyAspect.I);
            };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public int Weaved(ref int i)
         {
            i = 8;
            return i;
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static int I;

         public void After(int i)
         {
            I = i;
         }
      }
   }

   
}