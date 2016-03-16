using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.Samples.Parameters.Integers
{
   public class LowerThanSampleTest :
      NetAspectTest<LowerThanSampleTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            var classToWeave_L = new ClassToWeave();
            try
            {
                classToWeave_L.Check(10);
                Assert.Fail("Must fail");
            }
            catch (Exception)
            {
            } 
             try
            {
                classToWeave_L.Check(11);
                Assert.Fail("Must fail");
            }
            catch (Exception)
            {
            }
             classToWeave_L.Check(9);
         };
      }

      public class ClassToWeave
      {
          public void Check([LowerThan(10)] int param)
         {
             
         }
      }

      public class LowerThanAttribute : Attribute
      {
          private readonly int _value;
          public bool NetAspectAttribute = true;

          public LowerThanAttribute(int value)
          {
              _value = value;
          }

          public void BeforeMethodForParameter(ParameterInfo parameter, int parameterValue)
          {
              BeforeMethodForParameter(parameter, (long)parameterValue);
          }

          public void BeforeMethodForParameter(ParameterInfo parameter, long parameterValue)
          {
              if (parameterValue >= _value)
                  throw new Exception(string.Format("{0} must be lower than {1}", parameter.Name, _value));
          }
      }
   }
}
