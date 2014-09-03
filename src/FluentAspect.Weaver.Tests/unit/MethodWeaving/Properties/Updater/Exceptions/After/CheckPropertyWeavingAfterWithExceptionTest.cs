using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Exceptions.After
{
   public class CheckPropertyWeavingAfterWithExceptionTest :
      NetAspectTest<CheckPropertyWeavingAfterWithExceptionTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.Property);
            var classToWeave_L = new ClassToWeave();
            try
            {
               classToWeave_L.Weaved(classToWeave_L);
               Assert.Fail();
            }
            catch (Exception)
            {
               Assert.IsNull(MyAspect.Property);
            }
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public ClassToWeave Weaved(ClassToWeave toWeave)
         {
            throw new Exception();
         }
      }

      public class MyAspect : Attribute
      {
         public static PropertyInfo Property;
         public bool NetAspectAttribute = true;

         public void AfterPropertySetMethod(PropertyInfo Property)
         {
            Property = Property;
         }
      }
   }
}
