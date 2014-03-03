using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.After.ParameterName
{
    public class AfterCallMethodCallerParameterNameParameterWithObjectTypeTest : NetAspectTest<AfterCallMethodCallerParameterNameParameterWithObjectTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.AreEqual(0, MyAspect.ParameterName);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.Weaved(12);
               Assert.AreEqual(12, MyAspect.ParameterName);
            };
      }

      public class ClassToWeave
      {

          [MyAspect]
          public string Method() {return "Hello";}

         public string Weaved(int param1)
         {
             return Method();
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static object ParameterName;

         public void AfterCallMethod(object callerParam1)
         {
             ParameterName = callerParam1;
         }
      }
   }

   
}