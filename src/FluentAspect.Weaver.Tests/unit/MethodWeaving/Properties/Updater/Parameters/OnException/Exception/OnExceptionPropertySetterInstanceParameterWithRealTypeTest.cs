using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.OnException.Exception
{
    public class OnExceptionPropertySetterInstanceParameterWithRealTypeTest : NetAspectTest<OnExceptionPropertySetterInstanceParameterWithRealTypeTest.ClassToWeave>
   {
       protected override Action CreateEnsure()
       {
           return () =>
           {
               Assert.IsNull(MyAspect.Exception);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.MyProperty = "";
               Assert.AreEqual("Message", MyAspect.Exception.Message);
           };
       }


      public class ClassToWeave
      {
         [MyAspect]
         public string MyProperty
         {
             get { throw new System.Exception("Message"); }
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static System.Exception Exception;

          public void OnExceptionPropertyGet(System.Exception exception)
         {
             Exception = exception;
         }
      }
   }

   
}