using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Exceptions.OnException
{
   public class CheckMethodWeavingOnExceptionWithNoExceptionTest :
      NetAspectTest<CheckMethodWeavingOnExceptionWithNoExceptionTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.Method);
            var classToWeave_L = new ClassToWeave();
            try
            {
               classToWeave_L.Weaved(classToWeave_L);
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
         public ClassToWeave Weaved(ClassToWeave toWeave)
         {
            return toWeave;
         }
      }

      public class MyAspect : Attribute
      {
         public static MethodBase Method;
         public bool NetAspectAttribute = true;

         public void OnExceptionMethod(MethodBase method)
         {
            Method = method;
         }
      }
   }
}
