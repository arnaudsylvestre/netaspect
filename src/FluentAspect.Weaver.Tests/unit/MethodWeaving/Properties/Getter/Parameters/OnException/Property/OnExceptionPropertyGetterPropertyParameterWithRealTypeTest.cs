using System;
using System.Reflection;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters.OnException.Property
{
    public class OnExceptionPropertyGetterPropertyParameterWithRealTypeTest : NetAspectTest<OnExceptionPropertyGetterPropertyParameterWithRealTypeTest.ClassToWeave>
   {
       protected override Action CreateEnsure()
       {
           return () =>
           {
               Assert.IsNull(MyAspect.Property);
               var classToWeave_L = new ClassToWeave();
               try
               {
                   var property = classToWeave_L.MyProperty;
                   Assert.Fail();
               }
               catch (System.Exception)
               {
                   Assert.AreEqual("MyProperty", MyAspect.Property.Name);
               }
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