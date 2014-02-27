using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.OnException.Instance
{
    public class OnExceptionPropertySetterInstanceParameterWithRealTypeTest : NetAspectTest<OnExceptionPropertySetterInstanceParameterWithRealTypeTest.ClassToWeave>
   {
       protected override Action CreateEnsure()
       {
           return () =>
           {
               Assert.IsNull(MyAspect.Instance);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.MyProperty = "";
               Assert.AreEqual(classToWeave_L, MyAspect.Instance);
           };
       }


      public class ClassToWeave
      {
         [MyAspect]
         public string MyProperty
         {
             get { throw new System.Exception();}
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

          public static ClassToWeave Instance;

          public void OnExceptionPropertyGet(ClassToWeave instance)
         {
             Instance = instance;
         }
      }
   }

   
}