using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.Samples.Parameters.Integers
{
   public class GreaterThanSampleTest :
      NetAspectTest<GreaterThanSampleTest.ClassToWeave>
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
                classToWeave_L.Check(9);
                Assert.Fail("Must fail");
            }
            catch (Exception)
            {
            }
            try
            {
                classToWeave_L.CheckLong(9);
                Assert.Fail("Must fail");
            }
            catch (Exception)
            {
            }
             classToWeave_L.Check(11);
             classToWeave_L.CheckLong(11);
         };
      }

      public class ClassToWeave
      {
          public void Check([GreaterThan(10)] int param)
          {

          }
          public void CheckLong([GreaterThan(10)] long param)
          {

          }
      }

      public class GreaterThanAttribute : Attribute
      {
          private readonly int _value;
          public bool NetAspectAttribute = true;

          public GreaterThanAttribute(int value)
          {
              _value = value;
          }

          public void BeforeMethodForParameter(ParameterInfo parameter, int parameterValue)
          {
              BeforeMethodForParameter(parameter, (long)parameterValue);
          }

          public void BeforeMethodForParameter(ParameterInfo parameter, long parameterValue)
          {
              if (parameterValue <= _value)
                  throw new Exception(string.Format("{0} must be greater than {1}", parameter.Name, _value));
          }
      }
   }
}
