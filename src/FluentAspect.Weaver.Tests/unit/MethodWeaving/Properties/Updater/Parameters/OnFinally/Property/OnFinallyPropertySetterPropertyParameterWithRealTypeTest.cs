using System;
using System.Reflection;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Properties.Updater.Parameters.OnFinally.Property
{
    public class OnFinallyPropertySetterPropertyParameterWithRealTypeTest : NetAspectTest<OnFinallyPropertySetterPropertyParameterWithRealTypeTest.ClassToWeave>
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
             get { return ""; }
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static PropertyInfo Property;

         public void OnFinallyPropertyGet(PropertyInfo property)
         {
             Property = property;
         }
      }
   }

   
}