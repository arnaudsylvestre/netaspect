using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.After.Value
{
    public class AfterPropertySetterValueParameterWithRealTypeTest : NetAspectTest<AfterPropertySetterValueParameterWithRealTypeTest.ClassToWeave>
   {
       protected override Action CreateEnsure()
       {
           return () =>
           {
               Assert.IsNull(MyAspect.Value);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.MyProperty = "Value";
               Assert.AreEqual("Return", MyAspect.Value);
           };
       }


      public class ClassToWeave
      {
         [MyAspect]
         public string MyProperty
         {
             set {  }
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static string Value;

         public void BeforePropertyGet(string value)
         {
             Value = result;
         }
      }
   }

   
}