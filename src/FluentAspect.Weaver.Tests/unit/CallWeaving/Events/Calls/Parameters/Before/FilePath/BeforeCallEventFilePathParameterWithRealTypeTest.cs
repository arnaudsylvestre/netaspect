using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Events.Calls.Parameters.Before.FilePath
{
    public class BeforeCallEventFilePathParameterWithRealTypeTest : NetAspectTest<BeforeCallEventFilePathParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.AreEqual(null, MyAspect.FilePath);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.Weaved();
               Assert.AreEqual("", MyAspect.FilePath);
            };
      }

      public class ClassToWeave
      {

          [MyAspect]
          public event Action Event;

         public void Weaved()
         {
             Event();
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static string FilePath;

         public void BeforeRaiseEvent(string filePath)
         {
             FilePath = filePath;
         }
      }
   }

   
}