using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Exceptions.OnException
{
   public class CheckPropertyWeavingOnExceptionWithNoExceptionAndFinallyTest :
      NetAspectTest<CheckPropertyWeavingOnExceptionWithNoExceptionAndFinallyTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.Property);
            var classToWeave_L = new ClassToWeave();
            try
            {
               classToWeave_L.Weaved = classToWeave_L;
               Assert.Fail();
            }
            catch (Exception)
            {
               Assert.IsNull(MyAspect.Property);
               Assert.AreEqual("Weaved", MyAspect.FinallyProperty.Name);
            }
         };
      }

      public class ClassToWeave
      {
         public ClassToWeave toWeave;

         [MyAspect]
         public ClassToWeave Weaved
         {
            set { toWeave = value; }
         }
      }

      public class MyAspect : Attribute
      {
         public static PropertyInfo Property;
         public static PropertyInfo FinallyProperty;
         public bool NetAspectAttribute = true;

         public void OnExceptionPropertySetMethod(PropertyInfo property)
         {
            Property = property;
         }

         public void OnFinallyPropertySetMethod(PropertyInfo property)
         {
            FinallyProperty = property;
         }
      }
   }
}
