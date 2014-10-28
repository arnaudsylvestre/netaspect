using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MultiWeaving
{
   public class CheckWithIfMethodTest :
      NetAspectTest<CheckWithIfMethodTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            var classToWeave_L = new ClassToWeave();
            Assert.AreEqual("", classToWeave_L.Weaved(false));
            Assert.AreSame(classToWeave_L, MyAspect.Instance);
         };
      }


      public class ClassToWeave
      {
         [MyAspect]
         public string Weaved(bool accessField)
         {
             while (!accessField)
                accessField = true;
             return "";
         }
      }

      public class MyAspect : Attribute
      {
          public static object Instance;
         public bool NetAspectAttribute = true;

         public void AfterMethod(object instance)
         {
             Instance = instance;
         }
      }
   }
}
