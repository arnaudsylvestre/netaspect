using System;
using System.Reflection;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Parameters.OnException
{
    public class OnExceptionConstructorInstanceParameterWithRealTypeTest : NetAspectTest<OnExceptionConstructorInstanceParameterWithRealTypeTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
            {
               Assert.IsNull(MyAspect.Instance);
                try
                {
                    var classToWeave_L = new ClassToWeave();
                    Assert.Fail();
                }
                catch (Exception)
                {
                    Assert.NotNull(MyAspect.Instance);
                }
            };
      }

      public class ClassToWeave
      {
          [MyAspect]
          public ClassToWeave()
          {
              throw new Exception();
          }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public static ClassToWeave Instance;

         public void OnException(ClassToWeave instance)
         {
             Instance = instance;
         }
      }
   }

   
}