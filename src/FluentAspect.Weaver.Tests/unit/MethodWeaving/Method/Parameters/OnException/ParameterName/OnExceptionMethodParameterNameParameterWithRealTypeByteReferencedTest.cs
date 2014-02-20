using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.OnException.ParameterName
{
   public class OnExceptionMethodParameterNameParameterWithRealTypeByteReferencedTest : NetAspectTest<OnExceptionMethodParameterNameParameterWithRealTypeByteReferencedTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.AreEqual(0, MyAspect.I);
               var classToWeave_L = new ClassToWeave();
               byte i = 12;
               try
               {
                  classToWeave_L.Weaved(ref i);
                  Assert.Fail();
               }
               catch
               {

               }
               Assert.AreEqual(12, MyAspect.I);
            };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public void Weaved(ref byte i)
         {
            throw new Exception();
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static int I;

         public void OnException(byte i)
         {
            I = i;
         }
      }
   }

   
}