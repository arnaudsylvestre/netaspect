using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters.Before.Property
{
   public class BeforePropertyPropertyInfoParameterWithRealTypeTest :
      NetAspectTest<BeforePropertyPropertyInfoParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.PropertyInfo);
            var classToWeave_L = new ClassToWeave();
            string value = classToWeave_L.Weaved;
            Assert.AreEqual(classToWeave_L.GetType().GetProperty("Weaved"), MyAspect.PropertyInfo);
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public string Weaved
         {
            get { return "12"; }
         }
      }

      public class MyAspect : Attribute
      {
         public static PropertyInfo PropertyInfo;
         public bool NetAspectAttribute = true;

         public void BeforePropertyGetMethod(PropertyInfo property)
         {
            PropertyInfo = property;
         }
      }
   }
}
