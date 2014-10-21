using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.Selectors.Infos
{
   public class GetFieldWithSelectorFieldNameTest :
      NetAspectTest<GetFieldWithSelectorFieldNameTest.ClassToWeave>
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

         public string Weaved()
         {
            return Field;
         }
      }

      public class MyAspect : Attribute
      {
         public static ClassToWeave Caller;
         public bool NetAspectAttribute = true;

         public void BeforeGetField(ClassToWeave caller)
         {
            Caller = caller;
         }

         public static bool SelectField(FieldInfo field)
         {
            return field.Name == "Field";
         }
      }
   }
}
