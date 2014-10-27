using System;
using System.Collections.Generic;
using System.Reflection;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.ColumnNumber;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.Constructor
{
   public class AfterCallConstructorConstructorParameterWithBadTypeTest :
      NetAspectTest<AfterCallConstructorConstructorParameterWithBadTypeTest.ClassToWeave>
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
                           "the constructor parameter in the method AfterCallConstructor of the type '{0}' is declared with the type 'System.String' but it is expected to be System.Reflection.MethodBase",
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
         public bool NetAspectAttribute = true;

         public void AfterCallConstructor(string constructor)
         {
         }
      }
   }
}
