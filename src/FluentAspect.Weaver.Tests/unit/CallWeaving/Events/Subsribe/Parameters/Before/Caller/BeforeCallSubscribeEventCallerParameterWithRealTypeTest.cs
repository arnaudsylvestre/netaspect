using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Events.Subsribe.Parameters.Before.Caller
{
    public class BeforeCallSubscribeEventCallerParameterWithRealTypeTest : NetAspectTest<BeforeCallSubscribeEventCallerParameterWithRealTypeTest.ClassToWeave>
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
             Event += () => {};
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static ClassToWeave Caller;

         public void BeforeRaiseEvent(ClassToWeave caller)
         {
             Caller = caller;
         }
      }
   }

   
}