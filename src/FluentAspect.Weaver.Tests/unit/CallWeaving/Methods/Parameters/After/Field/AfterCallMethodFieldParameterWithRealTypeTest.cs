using System;
using System.Reflection;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Methods.Parameters.After.Field
{
    public class AfterCallMethodFieldParameterWithRealTypeTest : NetAspectTest<AfterCallMethodFieldParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.AreEqual(null, MyAspect.Field);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.Weaved();
               Assert.AreEqual("Field", MyAspect.Field.Name);
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

         public static FieldInfo Field;

         public void AfterCallMethod(FieldInfo columnNumber)
         {
             Field = columnNumber;
         }
      }
   }

   
}