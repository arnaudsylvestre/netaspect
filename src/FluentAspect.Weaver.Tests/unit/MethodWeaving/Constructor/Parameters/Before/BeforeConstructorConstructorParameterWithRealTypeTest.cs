using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.Before.Constructor
{
   public class BeforeConstructorConstructorParameterWithRealTypeTest :
      NetAspectTest<BeforeConstructorConstructorParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.Constructor);
            var classToWeave_L = new ClassToWeave();
            Assert.AreEqual(".ctor", MyAspect.Constructor.Name);
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
         public static MethodBase Constructor;
         public bool NetAspectAttribute = true;

         public void BeforeConstructor(MethodBase constructor)
         {
            Constructor = constructor;
         }
      }
   }
}
