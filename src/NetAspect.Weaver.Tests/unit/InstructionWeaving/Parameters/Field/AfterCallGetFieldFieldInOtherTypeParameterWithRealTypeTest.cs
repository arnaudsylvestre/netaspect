using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.Field
{
   public class AfterCallGetFieldFieldInOtherTypeParameterWithRealTypeTest :
      NetAspectTest<AfterCallGetFieldFieldInOtherTypeParameterWithRealTypeTest.ClassToWeave>
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

      public class ClassCalled
      {
         [MyAspect] public string Field;
      }

      public class ClassToWeave
      {
         private readonly ClassCalled called = new ClassCalled();

         public string Weaved()
         {
            return called.Field;
         }
      }

      public class MyAspect : Attribute
      {
         public static FieldInfo Field;
         public bool NetAspectAttribute = true;

         public void AfterGetField(FieldInfo field)
         {
            Field = field;
         }
      }
   }
}
