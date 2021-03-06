using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.Selectors.Members
{
   public class UpdateFieldWithSelectorFieldNameTest :
      NetAspectTest<UpdateFieldWithSelectorFieldNameTest.ClassToWeave>
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
         public string Field;

         public void Weaved()
         {
            Field = "Dummy";
         }
      }

      public class MyAspect : Attribute
      {
         public static ClassToWeave Caller;
         public bool NetAspectAttribute = true;

         public void BeforeUpdateField(ClassToWeave callerInstance)
         {
            Caller = callerInstance;
         }

         public static bool SelectField(FieldInfo field)
         {
            return field.Name == "Field";
         }
      }
   }
}
