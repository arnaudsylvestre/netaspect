using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Parameters.Constructor
{
   public class AfterConstructorConstructorInfoParameterWithRealTypeTest :
      NetAspectTest<AfterConstructorConstructorInfoParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.ConstructorInfo);
            var classToWeave_L = new ClassToWeave();
            Assert.AreEqual(".ctor", MyAspect.ConstructorInfo.Name);
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
         public static MethodBase ConstructorInfo;
         public bool NetAspectAttribute = true;

         public void AfterConstructor(MethodBase constructor)
         {
            ConstructorInfo = constructor;
         }
      }
   }
}
