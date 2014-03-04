using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Methods.Parameters.Before.Called
{
    public class BeforeCallMethodCalledParameterWithObjectTypeTest : NetAspectTest<BeforeCallMethodCalledParameterWithObjectTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.IsNull(MyAspect.Called);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.Weaved();
               Assert.AreEqual(classToWeave_L, MyAspect.Called);
            };
      }

      public class ClassToWeave
      {

          [MyAspect]
          public string Method() {return "Hello";}

         public string Weaved()
         {
             return Method();
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static object Called;

         public void BeforeCallMethod(object called)
         {
             Called = called;
         }
      }
   }

   
}