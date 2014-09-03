using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Exceptions.OnException
{
   public class CheckPropertyWeavingOnExceptionWithExceptionAndFinallyTest :
      NetAspectTest<CheckPropertyWeavingOnExceptionWithExceptionAndFinallyTest.ClassToWeave>
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
               Assert.AreEqual("Weaved", MyAspect.Property.Name);
               Assert.AreEqual("Weaved", MyAspect.FinallyProperty.Name);
            }
         };
      }

      public class ClassToWeave
      {
         [MyAspect]
         public ClassToWeave Weaved
         {
            set
            {
               if (value != null)
                  throw new Exception();
            }
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
