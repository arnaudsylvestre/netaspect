using System;
using System.Reflection;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.Before.Method
{
    public class BeforePropertySetterMethodParameterWithRealTypeTest : NetAspectTest<BeforePropertySetterMethodParameterWithRealTypeTest.ClassToWeave>
   {
       protected override Action CreateEnsure()
       {
           return () =>
           {
               Assert.IsNull(MyAspect.Method);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.MyProperty = "";
               Assert.AreEqual("set_MyProperty", MyAspect.Method.Name);
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

         public static MethodInfo Method;

          public void BeforePropertySet(MethodInfo method)
         {
             Method = method;
         }
      }
   }

   
}