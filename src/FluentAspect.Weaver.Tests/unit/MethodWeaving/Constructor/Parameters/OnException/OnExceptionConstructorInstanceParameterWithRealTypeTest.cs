using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.OnException.Instance
{
   public class OnExceptionConstructorInstanceParameterWithRealTypeTest :
      NetAspectTest<OnExceptionConstructorInstanceParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.Instance);
            try
            {
               var classToWeave_L = new ClassToWeave();
               Assert.Fail();
            }
            catch
            {
            }
            Assert.NotNull(MyAspect.Instance);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public ClassToWeave()
         {
            throw new Exception();
         }
      }

      public class MyAspect : Attribute
      {
         public static ClassToWeave Instance;
         public bool NetAspectAttribute = true;

         public void OnExceptionConstructor(ClassToWeave instance)
         {
            Instance = instance;
         }
      }
   }
}
