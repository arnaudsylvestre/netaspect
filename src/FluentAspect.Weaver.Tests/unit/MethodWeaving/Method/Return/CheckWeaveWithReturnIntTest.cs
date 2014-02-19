using System;
using System.Reflection;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Attributes
{
   public class CheckWeaveWithReturnIntTest : NetAspectTest<CheckWeaveWithReturnIntTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.IsNull(MyAspect.Method);
               var res = new ClassToWeave().Weaved();
               Assert.AreEqual("Weaved", MyAspect.Method.Name);
               Assert.AreEqual(12, res);

            };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public int Weaved()
         {
            return 12;
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