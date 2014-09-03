using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.After.Property
{
   public class AfterPropertyPropertyInfoParameterWithRealTypeTest :
      NetAspectTest<AfterPropertyPropertyInfoParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.PropertyInfo);
            var classToWeave_L = new ClassToWeave();
            classToWeave_L.Weaved = "12";
            Assert.AreEqual(classToWeave_L.GetType().GetProperty("Weaved"), MyAspect.PropertyInfo);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public string Weaved
         {
            set { }
         }
      }

      public class MyAspect : Attribute
      {
         public static PropertyInfo PropertyInfo;
         public bool NetAspectAttribute = true;

         public void AfterPropertySetMethod(PropertyInfo Property)
         {
            PropertyInfo = Property;
         }
      }
   }
}
