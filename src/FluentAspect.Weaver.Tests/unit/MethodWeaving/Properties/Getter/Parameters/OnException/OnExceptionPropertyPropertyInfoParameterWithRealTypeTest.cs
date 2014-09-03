using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters.OnException.Property
{
   public class OnExceptionPropertyPropertyInfoParameterWithRealTypeTest :
      NetAspectTest<OnExceptionPropertyPropertyInfoParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.PropertyInfo);
            var classToWeave_L = new ClassToWeave();
            try
            {
               string value = classToWeave_L.Weaved;
               Assert.Fail();
            }
            catch
            {
            }
            Assert.AreEqual(classToWeave_L.GetType().GetProperty("Weaved"), MyAspect.PropertyInfo);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public string Weaved
         {
            get { throw new Exception(); }
         }
      }

      public class MyAspect : Attribute
      {
         public static PropertyInfo PropertyInfo;
         public bool NetAspectAttribute = true;

         public void OnExceptionPropertyGetMethod(PropertyInfo property)
         {
            PropertyInfo = property;
         }
      }
   }
}
