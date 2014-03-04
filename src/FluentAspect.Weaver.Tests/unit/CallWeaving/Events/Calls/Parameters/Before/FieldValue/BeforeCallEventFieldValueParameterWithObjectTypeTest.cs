using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.Before.FieldValue
{
    public class BeforeCallEventFieldValueParameterWithObjectTypeTest : NetAspectTest<BeforeCallEventFieldValueParameterWithObjectTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.IsNull(MyAspect.FieldValue);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.Weaved();
               Assert.AreEqual(classToWeave_L, MyAspect.FieldValue);
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

         public static object FieldValue;

         public void BeforeRaiseEvent(object fieldValue)
         {
             FieldValue = fieldValue;
         }
      }
   }

   
}