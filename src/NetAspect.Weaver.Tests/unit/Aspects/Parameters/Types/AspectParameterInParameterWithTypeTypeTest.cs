using System;
using System.Collections.Generic;
using NUnit.Framework;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.Aspects.Parameters.Types
{
   public class AspectParameterInParameterWithTypeTypeTest :
      NetAspectTest<AspectParameterInParameterWithTypeTypeTest.ClassToWeave>
   {
      
       protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
       {
           return errorHandler => errorHandler.Add(
              new ErrorReport.Error
              {
                  Level = ErrorLevel.Error,
                  Message = string.Format("The parameter 'value' in a constructor of the aspect {0} can not be declared with the type System.Type. Only the following types are allowed : System.String, System.Int32, System.Boolean, System.Byte, System.Char, System.Double, System.Single, System.Int64, System.Int16, System.SByte, System.UInt32, System.UInt16, System.UInt64", typeof(MyAspect))
              });
       }


      public class ClassToWeave
      {

          public void Weaved([MyAspect(typeof(int))] int i)
         {
         }
      }

      public class MyAspect : Attribute
      {
          public static int I;
         public bool NetAspectAttribute = true;

          public MyAspect(Type value)
          {
          }

          public void AfterMethodForParameter(int parameterValue)
         {
            I = parameterValue;
         }
      }
   }
}
