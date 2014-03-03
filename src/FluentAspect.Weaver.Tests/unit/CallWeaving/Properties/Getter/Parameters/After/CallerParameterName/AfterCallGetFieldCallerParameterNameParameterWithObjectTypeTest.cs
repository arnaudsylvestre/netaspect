using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.After.ParameterName
{
    public class AfterCallGetPropertyCallerParameterNameParameterWithObjectTypeTest : NetAspectTest<AfterCallGetPropertyCallerParameterNameParameterWithObjectTypeTest.ClassToWeave>
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
             return Field;
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static object ParameterName;

         public void AfterGetProperty(object callerParam1)
         {
             ParameterName = callerParam1;
         }
      }
   }

   
}