using System;
using System.Reflection;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.OnException.Property
{
    public class OnExceptionPropertySetterPropertyParameterWithRealTypeTest : NetAspectTest<OnExceptionPropertySetterPropertyParameterWithRealTypeTest.ClassToWeave>
   {
       protected override Action CreateEnsure()
       {
           return () =>
           {
               Assert.IsNull(MyAspect.Property);
               var classToWeave_L = new ClassToWeave();
               classToWeave_L.MyProperty = "";
               Assert.AreEqual("MyProperty", MyAspect.Property.Name);
           };
       }


      public class ClassToWeave
      {
         [MyAspect]
         public string MyProperty
          {
              get { throw new System.Exception(); }
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static PropertyInfo Property;

         public void OnExceptionPropertyGet(PropertyInfo property)
         {
             Property = property;
         }
      }
   }

   
}