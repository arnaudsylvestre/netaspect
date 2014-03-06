using System;
using System.Reflection;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters.OnFinally.Method
{
    public class OnFinallyPropertyGetterMethodParameterWithRealTypeTest : NetAspectTest<OnFinallyPropertyGetterMethodParameterWithRealTypeTest.ClassToWeave>
   {
       protected override Action CreateEnsure()
       {
           return () =>
           {
               Assert.IsNull(MyAspect.Method);
               var classToWeave_L = new ClassToWeave();
               var property = classToWeave_L.MyProperty;
               Assert.AreEqual("get_MyProperty", MyAspect.Method.Name);
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

         public static MethodInfo Method;

          public void OnFinallyPropertyGet(MethodInfo method)
         {
             Method = method;
         }
      }
   }

   
}