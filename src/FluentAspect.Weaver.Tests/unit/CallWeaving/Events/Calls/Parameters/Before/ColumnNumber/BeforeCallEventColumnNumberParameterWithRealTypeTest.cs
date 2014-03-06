using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Events.Calls.Parameters.Before.ColumnNumber
{
    public class BeforeCallEventColumnNumberParameterWithRealTypeTest : NetAspectTest<BeforeCallEventColumnNumberParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.AreEqual(0, MyAspect.ColumnNumber);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.Weaved();
               Assert.AreEqual(12, MyAspect.ColumnNumber);
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

         public static int ColumnNumber;

         public void BeforeRaiseEvent(int columnNumber)
         {
             ColumnNumber = columnNumber;
         }
      }
   }

   
}