using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Exceptions.OnException
{
   public class CheckMethodWeavingOnExceptionWithExceptionAndFinallyTest :
      NetAspectTest<CheckMethodWeavingOnExceptionWithExceptionAndFinallyTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.Constructor);
            try
            {
               var classToWeave_L = new ClassToWeave();
               Assert.Fail();
            }
            catch (Exception)
            {
               Assert.AreEqual(".ctor", MyAspect.Constructor.Name);
               Assert.AreEqual(".ctor", MyAspect.FinallyConstructor.Name);
            }
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public ClassToWeave()
         {
            throw new Exception();
            //return toWeave;
         }
      }

      public class MyAspect : Attribute
      {
         public static MethodBase Constructor;
         public static MethodBase FinallyConstructor;
         public bool NetAspectAttribute = true;

         public void OnExceptionConstructor(MethodBase constructor)
         {
            Constructor = constructor;
         }

         public void OnFinallyConstructor(MethodBase constructor)
         {
            FinallyConstructor = constructor;
         }
      }
   }
}
