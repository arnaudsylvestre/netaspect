using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.Before.ParameterName
{
   public class BeforeMethodParameterNameParameterWithRealTypeByteTest : NetAspectTest<BeforeMethodParameterNameParameterWithRealTypeByteTest.ClassToWeave>
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
         public void Weaved(byte i)
         {

         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static int I;

         public void Before(byte i)
         {
            I = i;
         }
      }
   }

   
}