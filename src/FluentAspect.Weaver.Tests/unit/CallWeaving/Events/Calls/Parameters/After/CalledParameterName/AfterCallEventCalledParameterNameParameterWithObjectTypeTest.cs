using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.After.CalledParameterName
{
    public class AfterCallEventCalledParameterNameParameterWithObjectTypeTest : NetAspectTest<AfterCallEventCalledParameterNameParameterWithObjectTypeTest.ClassToWeave>
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
             Event();
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static object ParameterName;

         public void AfterRaiseEvent(object calledParam1)
         {
             ParameterName = calledParam1;
         }
      }
   }

   
}