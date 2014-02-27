using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.After.Instance
{
    public class AfterGetFieldCallerParameterWithRealTypeTest : NetAspectTest<AfterGetFieldCallerParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
                Assert.IsNull(MyAspect.Caller);
               var classToCallWeaved = new ClassToCallWeaved();
               classToCallWeaved.CallWeaved();
               Assert.AreEqual(classToCallWeaved, MyAspect.Caller);
            };
      }

      public class ClassToWeave
      {

          [MyAspect]
          public void WeavedForCall()
          {

          }
      }

      public class ClassToCallWeaved
      {
          public void CallWeaved()
          {
              new ClassToWeave().WeavedForCall();
          }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static ClassToCallWeaved Caller;

         public void AfterCall(ClassToCallWeaved caller)
         {
             Caller = caller;
         }
      }
   }

   
}