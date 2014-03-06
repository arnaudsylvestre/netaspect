using System;
using System.Reflection;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Return
{
   public class CheckWeaveWithReturnVoidTest : NetAspectTest<CheckWeaveWithReturnVoidTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.IsNull(MyAspect.Method);
               new ClassToWeave().Weaved();
               Assert.AreEqual("Weaved", MyAspect.Method.Name);
            };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public void Weaved()
         {

         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static MethodInfo Method;

         public void Before(MethodInfo method)
         {
             Method = method;
         }
      }
   }

   
}