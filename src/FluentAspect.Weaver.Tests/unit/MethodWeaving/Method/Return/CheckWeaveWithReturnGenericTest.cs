using System;
using System.Reflection;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Return
{
   public class CheckWeaveWithReturnGenericTest : NetAspectTest<CheckWeaveWithReturnGenericTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.IsNull(MyAspect.Method);
               var classToWeave_L = new ClassToWeave();
               var res = classToWeave_L.Weaved(classToWeave_L);
               Assert.AreEqual("Weaved", MyAspect.Method.Name);
               Assert.AreEqual(classToWeave_L, res);

            };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public T Weaved<T>(T toWeave)
         {
            return toWeave;
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