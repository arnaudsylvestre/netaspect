using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Instance
{
   public class AfterPropertyGetterInstanceParameterWithBadTypeTest : NetAspectTest<AfterMethodInstanceParameterWithBadTypeTest.ClassToWeave>
   {
       protected override Action CreateEnsure()
       {
           return () =>
           {
               Assert.IsNull(MyAspect.Instance);
               var classToWeave_L = new ClassToWeave();
               var property = classToWeave_L.MyProperty;
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

         public void BeforeGetProperty(ClassToWeave instance)
         {
             Instance = instance;
         }
      }
   }

   
}