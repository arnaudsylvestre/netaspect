using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.OnException.Instance
{
   public class OnExceptionMethodInstanceParameterWithRealTypeTest :
      NetAspectTest<OnExceptionMethodInstanceParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.Instance);
            var classToWeave_L = new ClassToWeave();
            try
            {
               classToWeave_L.Weaved();
               Assert.Fail();
            }
            catch
            {
            }
            Assert.AreEqual(classToWeave_L, MyAspect.Instance);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public void Weaved()
         {
            throw new Exception();
         }
      }

      public class MyAspect : Attribute
      {
         public static ClassToWeave Instance;
         public bool NetAspectAttribute = true;

         public void OnException(ClassToWeave instance)
         {
            Instance = instance;
         }
      }
   }
}
