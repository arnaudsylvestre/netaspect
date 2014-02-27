using System;
using System.Reflection;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.Before
{
    public class BeforeConstructorInstanceParameterWithRealTypeTest : NetAspectTest<BeforeConstructorInstanceParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.IsNull(MyAspect.Instance);
               var classToWeave_L = new ClassToWeave();
               Assert.AreEqual(classToWeave_L, MyAspect.Instance);
            };
      }

      public class ClassToWeave
      {
          [MyAspect]
          public ClassToWeave()
          {
              
          }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static ClassToWeave Instance;

         public void Before(ClassToWeave instance)
         {
             Instance = instance;
         }
      }
   }

   
}