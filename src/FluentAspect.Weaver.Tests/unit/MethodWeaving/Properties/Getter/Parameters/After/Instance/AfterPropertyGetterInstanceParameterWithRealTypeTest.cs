using System;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Properties.Getter.Parameters.After.Instance
{
    public class AfterPropertyGetterInstanceParameterWithRealTypeTest : NetAspectTest<AfterPropertyGetterInstanceParameterWithRealTypeTest.ClassToWeave>
   {
       protected override Action CreateEnsure()
       {
           return () =>
           {
               Assert.IsNull(MyAspect.Instance);
               var classToWeave_L = new ClassToWeave();
               var property = classToWeave_L.MyProperty;
               Assert.AreEqual(classToWeave_L, MyAspect.Instance);
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

          public static ClassToWeave Instance;

          public void AfterPropertyGet(ClassToWeave instance)
         {
             Instance = instance;
         }
      }
   }

   
}