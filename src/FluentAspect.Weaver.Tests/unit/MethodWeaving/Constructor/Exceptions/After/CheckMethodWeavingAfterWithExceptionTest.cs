using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Exceptions.After
{
   public class CheckMethodWeavingAfterWithExceptionTest :
      NetAspectTest<CheckMethodWeavingAfterWithExceptionTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.Method);
            try
            {
               var classToWeave_L = new ClassToWeave();
               Assert.Fail();
            }
            catch (Exception)
            {
               Assert.IsNull(MyAspect.Method);
            }
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
         public static MethodBase Method;
         public bool NetAspectAttribute = true;

         public void AfterConstructor(MethodBase constructor)
         {
            Method = constructor;
         }
      }
   }
}
