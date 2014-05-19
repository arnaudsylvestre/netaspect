using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Method.Parameters.Before.Instance
{
   public class BeforeConstructorParameterValueWithRealTypeWithBaseCallReferencedTest :
        NetAspectTest<BeforeConstructorParameterValueWithRealTypeWithBaseCallReferencedTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                   Assert.IsNull(MyAspect.ParameterValue);
                   var classToWeave_L = new ClassToWeave("value");
                   Assert.AreEqual("ModifiedByAspect", classToWeave_L.BaseClass);
                   Assert.AreEqual("value", MyAspect.ParameterValue);
                };
        }


        public class BaseClassToWeave
        {
           public string BaseClass;

           public BaseClassToWeave(string baseClass)
           {
              BaseClass = baseClass;
           }
        }

        public class ClassToWeave : BaseClassToWeave
        {

           public ClassToWeave([MyAspect] string p):base(p)
        {

        }
        }

        public class MyAspect : Attribute
        {
            public static string ParameterValue;
            public bool NetAspectAttribute = true;

            public void BeforeConstructorForParameter(ref string parameterValue)
            {
               ParameterValue = parameterValue;
               parameterValue = "ModifiedByAspect";
            }
        }
    }
}