using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.OnFinally.Instance
{
    public class OnFinallyPropertySetterInstanceParameterWithRealTypeTest : NetAspectTest<OnFinallyPropertySetterInstanceParameterWithRealTypeTest.ClassToWeave>
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
             get { return ""; }
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

          public static ClassToWeave Instance;

          public void OnFinallyPropertyGet(ClassToWeave instance)
         {
             Instance = instance;
         }
      }
   }

   
}