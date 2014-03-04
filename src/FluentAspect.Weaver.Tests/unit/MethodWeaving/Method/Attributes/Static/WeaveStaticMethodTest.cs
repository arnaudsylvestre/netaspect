using System;
using System.Reflection;
using FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Attributes.Visibility;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Attributes.Static
{
    public class WeaveStaticMethodTest : NetAspectTest<WeavePublicMethodTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.IsNull(MyAspect.Method);
               ClassToWeave.Weaved();
               Assert.AreEqual("Weaved", MyAspect.Method.Name);
            };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public static void Weaved()
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