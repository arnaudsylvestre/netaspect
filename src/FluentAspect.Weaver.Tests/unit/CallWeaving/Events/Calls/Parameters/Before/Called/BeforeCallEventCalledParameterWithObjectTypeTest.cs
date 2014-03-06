using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Events.Calls.Parameters.Before.Called
{
    public class BeforeCallEventCalledParameterWithObjectTypeTest : NetAspectTest<BeforeCallEventCalledParameterWithObjectTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.IsNull(MyAspect.Called);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.Weaved();
               Assert.AreEqual(classToWeave_L, MyAspect.Called);
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

         public static object Called;

         public void BeforeRaiseEvent(object called)
         {
             Called = called;
         }
      }
   }

   
}