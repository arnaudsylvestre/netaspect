using System;
using System.Reflection;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.Before.Field
{
    public class BeforeCallEventFieldParameterWithRealTypeTest : NetAspectTest<BeforeCallEventFieldParameterWithRealTypeTest.ClassToWeave>
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
          public event Action Event;

         public void Weaved()
         {
             Event();
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static FieldInfo Field;

         public void BeforeRaiseEvent(FieldInfo columnNumber)
         {
             Field = columnNumber;
         }
      }
   }

   
}