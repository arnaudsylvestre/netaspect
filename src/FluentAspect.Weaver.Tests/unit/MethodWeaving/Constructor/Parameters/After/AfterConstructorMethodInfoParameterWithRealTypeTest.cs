using System;
using System.Reflection;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.After
{
    public class AfterConstructorMethodInfoParameterWithRealTypeTest : NetAspectTest<AfterConstructorMethodInfoParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.IsNull(MyAspect.MethodInfo);
               var classToWeave_L = new ClassToWeave();
               Assert.AreEqual(".ctor", MyAspect.MethodInfo.Name);
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

         public static MethodBase MethodInfo;

         public void After(MethodBase method)
         {
            MethodInfo = method;
         }
      }
   }

   
}