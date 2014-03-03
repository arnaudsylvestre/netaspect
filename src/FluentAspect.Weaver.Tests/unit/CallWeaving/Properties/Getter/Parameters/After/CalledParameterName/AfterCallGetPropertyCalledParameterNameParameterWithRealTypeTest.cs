using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Properties.Getter.Parameters.After.CalledParameterName
{
    public class AfterCallGetPropertyCalledParameterNameParameterWithRealTypeTest : NetAspectTest<AfterCallGetPropertyCalledParameterNameParameterWithRealTypeTest.ClassToWeave>
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
          public string Property {get;set;}

         public string Weaved(int param1)
         {
             return Property;
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static int ParameterName;

         public void AfterGetProperty(int calledParam1)
         {
             ParameterName = calledParam1;
         }
      }
   }

   
}