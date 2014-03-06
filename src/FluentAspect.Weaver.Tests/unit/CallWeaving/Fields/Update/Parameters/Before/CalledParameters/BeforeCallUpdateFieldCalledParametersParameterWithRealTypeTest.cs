using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Update.Parameters.Before.CalledParameters
{
    public class BeforeCallUpdateFieldCalledParametersParameterWithRealTypeTest : NetAspectTest<BeforeCallUpdateFieldCalledParametersParameterWithRealTypeTest.ClassToWeave>
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
          public string Field;

         public void Weaved(int param1, int param2)
         {
             Field = "Hello";
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static object[] CalledParameters;

         public void BeforeUpdateField(object[] calledParameters)
         {
             CalledParameters = calledParameters;
         }
      }
   }

   
}