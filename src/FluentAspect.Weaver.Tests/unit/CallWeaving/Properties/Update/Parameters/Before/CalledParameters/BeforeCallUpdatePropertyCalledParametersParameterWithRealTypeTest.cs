using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.Before.CalledParameters
{
    public class BeforeCallUpdatePropertyCalledParametersParameterWithRealTypeTest : NetAspectTest<BeforeCallUpdatePropertyCalledParametersParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.IsNull(MyAspect.CalledParameters);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.Weaved(1, 2);
               Assert.AreEqual(new object[]
                   {
                       1,2
                   }, MyAspect.CalledParameters);
            };
      }

      public class ClassToWeave
      {

          [MyAspect]
          public string Property {get;set;}

         public void Weaved(int param1, int param2)
         {
             Property = "Hello";
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static object[] CalledParameters;

         public void BeforeUpdateProperty(object[] calledParameters)
         {
             CalledParameters = calledParameters;
         }
      }
   }

   
}