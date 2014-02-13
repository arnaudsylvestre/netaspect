using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Result
{
   public class AfterMethodResultParameterWithRealTypeOutTest : NetAspectTest<AfterMethodResultParameterWithRealTypeOutTest.ClassToWeave>
   {
       protected override Action CreateEnsure()
       {
           return () =>
           {
               var classToWeave_L = new ClassToWeave();
               var res = classToWeave_L.Weaved();
               Assert.AreEqual("MyNewValue", res);
           };
       }

      public class ClassToWeave
      {
         [MyAspect]
         public string Weaved()
         {
             return "NeverUsedValue";
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public void After(out string instance)
         {
            instance = "MyNewValue";
         }
      }
   }

   
}