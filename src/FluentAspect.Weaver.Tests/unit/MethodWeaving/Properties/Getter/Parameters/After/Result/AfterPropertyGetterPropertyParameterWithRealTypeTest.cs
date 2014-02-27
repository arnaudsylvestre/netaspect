using System;
using System.Reflection;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters.After
{
    public class AfterPropertyGetterResultParameterWithRealTypeTest : NetAspectTest<AfterPropertyGetterResultParameterWithRealTypeTest.ClassToWeave>
   {
       protected override Action CreateEnsure()
       {
           return () =>
           {
               Assert.IsNull(MyAspect.Result);
               var classToWeave_L = new ClassToWeave();
               var property = classToWeave_L.MyProperty;
               Assert.AreEqual("Return", MyAspect.Result);
           };
       }


      public class ClassToWeave
      {
         [MyAspect]
         public string MyProperty
         {
             get { return "Return"; }
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static string Result;

         public void BeforePropertyGet(string result)
         {
             Result = result;
         }
      }
   }

   
}