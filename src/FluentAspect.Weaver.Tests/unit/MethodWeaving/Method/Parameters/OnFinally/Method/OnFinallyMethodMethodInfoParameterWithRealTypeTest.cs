using System;
using System.Reflection;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.OnFinally.Method
{
   public class OnFinallyMethodMethodInfoParameterWithRealTypeTest : NetAspectTest<OnFinallyMethodMethodInfoParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.IsNull(MyAspect.MethodInfo);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.Weaved();
               Assert.AreEqual(classToWeave_L.GetType().GetMethod("Weaved"), MyAspect.MethodInfo);
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

         public static MethodInfo MethodInfo;

         public void OnFinally(MethodInfo method)
         {
            MethodInfo = method;
         }
      }
   }

   
}