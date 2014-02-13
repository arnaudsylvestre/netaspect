using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Result
{
   public class AfterMethodResultParameterWithObjectTypeTest : NetAspectTest<AfterMethodResultParameterWithObjectTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.IsNull(MyAspect.Result);
               var classToWeave_L = new ClassToWeave();
               var res = classToWeave_L.Weaved();
               Assert.AreEqual(res, MyAspect.Result);
            };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public string Weaved()
         {
             return "Hello";
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static object Result;

         public void After(object instance)
         {
            Result = instance;
         }
      }
   }

   
}