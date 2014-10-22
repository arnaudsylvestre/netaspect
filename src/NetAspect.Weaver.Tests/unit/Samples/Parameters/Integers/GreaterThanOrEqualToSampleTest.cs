using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.Samples.Parameters.Integers
{
   public class GreaterThanOrEqualToSampleTest :
      NetAspectTest<GreaterThanOrEqualToSampleTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            var classToWeave_L = new ClassToWeave();
            
                classToWeave_L.Check(10);
               
             try
            {
                classToWeave_L.Check(9);
                Assert.Fail("Must fail");
            }
            catch (Exception e)
            {
            }
             classToWeave_L.Check(11);
         };
      }

      public class ClassToWeave
      {
          public void Check([GreaterThanOrEqualTo(10)] int param)
         {
             
         }
      }

      public class GreaterThanOrEqualToAttribute : Attribute
      {
          private readonly int _value;
          public bool NetAspectAttribute = true;

          public GreaterThanOrEqualToAttribute(int value)
          {
              _value = value;
          }

          public void BeforeMethodForParameter(ParameterInfo parameter, int parameterValue)
         {
            if (parameterValue < _value)
                throw new Exception(string.Format("{0} must be greater than or equal to {1}", parameter.Name, _value));
         }
      }
   }
}
