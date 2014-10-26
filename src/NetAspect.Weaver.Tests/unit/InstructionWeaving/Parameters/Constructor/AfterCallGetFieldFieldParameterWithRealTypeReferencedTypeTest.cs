using System;
using System.Collections.Generic;
using System.Reflection;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Parameters.Constructor
{
   public class AfterCallGetFieldFieldParameterWithRealTypeReferencedTypeTest :
      NetAspectTest<AfterCallGetFieldFieldParameterWithRealTypeReferencedTypeTest.ClassToWeave>
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
                           "impossible to ref/out the parameter 'field' in the method AfterGetField of the type '{0}'",
                           typeof (MyAspect).FullName)
                  });
      }

      public class ClassToWeave
      {
         [MyAspect] public string Field;

         public string Weaved()
         {
            return Field;
         }
      }

      public class MyAspect : Attribute
      {
         public bool NetAspectAttribute = true;

         public void AfterGetField(ref FieldInfo field)
         {
         }
      }
   }
}
