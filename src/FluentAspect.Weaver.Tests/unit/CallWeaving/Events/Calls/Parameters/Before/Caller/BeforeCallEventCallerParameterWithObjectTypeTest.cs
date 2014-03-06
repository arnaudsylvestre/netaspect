using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Events.Calls.Parameters.Before.Caller
{
    public class BeforeCallEventCallerParameterWithObjectTypeTest : NetAspectTest<BeforeCallEventCallerParameterWithObjectTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.IsNull(MyAspect.Caller);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.Weaved();
               Assert.AreEqual(classToWeave_L, MyAspect.Caller);
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

         public static object Caller;

         public void BeforeRaiseEvent(object caller)
         {
             Caller = caller;
         }
      }
   }

   
}