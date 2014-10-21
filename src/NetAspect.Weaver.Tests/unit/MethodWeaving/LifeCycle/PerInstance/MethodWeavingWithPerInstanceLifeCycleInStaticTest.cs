using System;
using System.Collections.Generic;
using NUnit.Framework;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.LifeCycle
{
   public class MethodWeavingWithPerInstanceLifeCycleInStaticTest :
      NetAspectTest<MethodWeavingWithPerInstanceLifeCycleInStaticTest.ClassToWeave>
   {

       protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
       {
           return errorHandler => errorHandler.Add(
              new ErrorReport.Error
              {
                  Level = ErrorLevel.Error,
                  Message = string.Format("Impossible to use the aspect {0} in the method Weaved of type {1} because this method is static and the aspect is declared as PerInstance",
                  typeof(MyAspect).FullName,
                  typeof(ClassToWeave).FullName)
              });
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
         public static List<MyAspect> aspects = new List<MyAspect>();
         public static string LifeCycle = "PerInstance";
         public bool NetAspectAttribute = true;
         public int nbBefore = 0;

         public MyAspect()
         {
            aspects.Add(this);
         }

         public void BeforeMethod()
         {
            nbBefore++;
         }
      }
   }
}
