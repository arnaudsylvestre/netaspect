using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.Atttributes.Statics
{
   public class WeaveStaticMethodTest : NetAspectTest<WeaveStaticMethodTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.Constructor);
            new ClassToWeave();
            Assert.AreEqual(".cctor", MyAspect.Constructor.Name);
            Assert.AreEqual(2, ClassToWeave.i);
         };
      }

      public class ClassToWeave
      {
          public static int i = 2;

          [MyAspect]
          static ClassToWeave()
          {
              
          }
      }

      public class MyAspect : Attribute
      {
         public static MethodBase Constructor;
         public bool NetAspectAttribute = true;

         public void BeforeConstructor(ConstructorInfo constructor)
         {
            Constructor = constructor;
         }
      }
   }
}
