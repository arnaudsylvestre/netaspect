using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.After.Caller
{
    public class AfterCallGetFieldCallerParameterWithObjectTypeTest : NetAspectTest<AfterCallGetFieldCallerParameterWithObjectTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.IsNull(MyAspect.Caller);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.Weaved();
               Assert.AreEqual(classToWeave_L, MyAspect.Caller);
            };
      }

      public class ClassToWeave
      {

          [MyAspect]
          public string Field;

         public string Weaved()
         {
             return Field;
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static object Caller;

         public void AfterGetField(object caller)
         {
             Caller = caller;
         }
      }
   }

   
}