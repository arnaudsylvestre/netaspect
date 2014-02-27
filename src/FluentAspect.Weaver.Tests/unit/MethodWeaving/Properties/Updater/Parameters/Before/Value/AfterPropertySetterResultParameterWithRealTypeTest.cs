using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.Before.Result
{
    public class BeforePropertySetterValueParameterWithRealTypeTest : NetAspectTest<BeforePropertySetterValueParameterWithRealTypeTest.ClassToWeave>
   {
       protected override Action CreateEnsure()
       {
           return () =>
           {
               Assert.IsNull(MyAspect.Result);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.MyProperty = "Value";
               Assert.AreEqual("Value", MyAspect.Result);
           };
       }


      public class ClassToWeave
      {
         [MyAspect]
         public string MyProperty
          {
              set { }
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static string Result;

         public void BeforePropertyGet(string value)
         {
             Result = result;
         }
      }
   }

   
}