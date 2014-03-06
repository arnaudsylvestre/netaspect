using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Events.Subsribe.Parameters.Before.CalledParameterName
{
    public class BeforeCallSubscribeEventCalledParameterNameParameterWithObjectTypeTest : NetAspectTest<BeforeCallSubscribeEventCalledParameterNameParameterWithObjectTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.AreEqual(0, MyAspect.ParameterName);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.Weaved(12);
               Assert.AreEqual(12, MyAspect.ParameterName);
            };
      }

      public class ClassToWeave
      {

          [MyAspect]
          public event Action Event;

         public void Weaved(int param1)
         {
             Event += () => {};
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static object ParameterName;

         public void BeforeRaiseEvent(object calledParam1)
         {
             ParameterName = calledParam1;
         }
      }
   }

   
}