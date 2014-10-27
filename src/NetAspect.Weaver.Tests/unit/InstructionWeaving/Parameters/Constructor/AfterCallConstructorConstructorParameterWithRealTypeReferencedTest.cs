using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.Constructor
{
   public class AfterCallConstructorConstructorParameterWithRealTypeReferencedTest :
      NetAspectTest<AfterCallConstructorConstructorParameterWithRealTypeReferencedTest.ClassToWeave>
   {
       protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
       {
           return
              errorHandler =>
                 errorHandler.Add(
                    new ErrorReport.Error
                    {
                        Level = ErrorLevel.Error,
                        Message =
                           string.Format(
                              "impossible to ref/out the parameter 'constructor' in the method AfterCallConstructor of the type '{0}'",
                              typeof(MyAspect).FullName)
                    });
       }

      public class ClassToWeave
      {


         [MyAspect]
         public ClassToWeave()
         {

         }

         public static ClassToWeave Create()
         {
            return new ClassToWeave();
         }
      }

      public class MyAspect : Attribute
      {
         public static MethodBase Constructor;
         public bool NetAspectAttribute = true;

         public void AfterCallConstructor(ref MethodBase constructor)
         {
            Constructor = constructor;
         }
      }
   }
}
