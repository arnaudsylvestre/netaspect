using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Attributes
{
    public class WeaveProtectedMethodTest : NetAspectTest<WeaveProtectedMethodTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.IsNull(MyAspect.Instance);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.CallWeaved();
               Assert.AreEqual(classToWeave_L, MyAspect.Instance);
            };
      }

      public class ClassToWeave
      {
          public void CallWeaved()
          {
              Weaved();
          }

         [MyAspect]
         protected void Weaved()
         {

         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static ClassToWeave Instance;

         public void Before(ClassToWeave instance)
         {
            Instance = instance;
         }
      }
   }

   
}