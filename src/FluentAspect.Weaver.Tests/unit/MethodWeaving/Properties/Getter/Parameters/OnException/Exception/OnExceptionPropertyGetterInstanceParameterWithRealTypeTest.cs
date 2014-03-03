using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters.OnException.Exception
{
    public class OnExceptionPropertyGetterInstanceParameterWithRealTypeTest : NetAspectTest<OnExceptionPropertyGetterInstanceParameterWithRealTypeTest.ClassToWeave>
   {
       protected override Action CreateEnsure()
       {
           return () =>
           {
               Assert.IsNull(MyAspect.Exception);
               var classToWeave_L = new ClassToWeave();
               try
               {
                   var property = classToWeave_L.MyProperty;
                   Assert.Fail();
               }
               catch (System.Exception)
               {
                   Assert.AreEqual("Message", MyAspect.Exception.Message);
               }
               
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