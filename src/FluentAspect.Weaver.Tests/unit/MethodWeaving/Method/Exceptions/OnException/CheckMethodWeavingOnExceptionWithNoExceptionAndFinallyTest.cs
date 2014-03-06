using System;
using System.Reflection;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Exceptions.OnException
{
   public class CheckMethodWeavingOnExceptionWithNoExceptionAndFinallyTest : NetAspectTest<CheckMethodWeavingOnExceptionWithNoExceptionAndFinallyTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
                  Assert.IsNull(MyAspect.Method);
                  var classToWeave_L = new ClassToWeave();
               try
               {
                  classToWeave_L.Weaved(classToWeave_L);
                  Assert.Fail();

               }
               catch (Exception)
               {
                  Assert.IsNull(MyAspect.Method);
                  Assert.AreEqual("Weaved", MyAspect.FinallyMethod.Name);
                  
               }

            };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public ClassToWeave Weaved(ClassToWeave toWeave)
         {
            return toWeave;
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static MethodInfo Method;
         public static MethodInfo FinallyMethod;

         public void OnException(MethodInfo method)
         {
            Method = method;
         }

         public void OnFinally(MethodInfo method)
         {
            FinallyMethod = method;
         }
      }
   }

   
}