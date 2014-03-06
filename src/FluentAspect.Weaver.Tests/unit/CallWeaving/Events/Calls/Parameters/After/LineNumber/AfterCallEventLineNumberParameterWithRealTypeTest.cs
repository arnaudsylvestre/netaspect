using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Events.Calls.Parameters.After.LineNumber
{
    public class AfterCallEventLineNumberParameterWithRealTypeTest : NetAspectTest<AfterCallEventLineNumberParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.AreEqual(0, MyAspect.LineNumber);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.Weaved();
               Assert.AreEqual(12, MyAspect.LineNumber);
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

         public static int LineNumber;

         public void AfterRaiseEvent(int lineNumber)
         {
             LineNumber = lineNumber;
         }
      }
   }

   
}