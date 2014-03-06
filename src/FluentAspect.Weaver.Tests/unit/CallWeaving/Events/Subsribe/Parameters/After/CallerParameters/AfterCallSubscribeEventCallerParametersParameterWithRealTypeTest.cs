using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Events.Subsribe.Parameters.After.CallerParameters
{
    public class AfterCallSubscribeEventCallerParametersParameterWithRealTypeTest : NetAspectTest<AfterCallSubscribeEventCallerParametersParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.IsNull(MyAspect.CallerParameters);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.Weaved(1, 2);
               Assert.AreEqual(new object[]
                   {
                       1,2
                   }, MyAspect.CallerParameters);
            };
      }

      public class ClassToWeave
      {

          [MyAspect]
          public event Action Event;

         public void Weaved(int param1, int param2)
         {
             Event += () => {};
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static object[] CallerParameters;

         public void AfterSubscribeEvent(object[] callerParameters)
         {
             CallerParameters = callerParameters;
         }
      }
   }

   
}