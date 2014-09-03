using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Exceptions.After
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
               ClassToWeave value = classToWeave_L.Weaved;
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
         public ClassToWeave Weaved
         {
            get { throw new Exception(); }
         }
      }

      public class MyAspect : Attribute
      {
         public static PropertyInfo Property;
         public bool NetAspectAttribute = true;

         public void AfterPropertyGetMethod(PropertyInfo property)
         {
            Property = property;
         }
      }
   }
}
