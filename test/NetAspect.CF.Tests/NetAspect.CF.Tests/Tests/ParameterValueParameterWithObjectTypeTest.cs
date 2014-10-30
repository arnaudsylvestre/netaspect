
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsParameterValueParameterWithObjectTypeTest
{

	[TestFixture]
	public class TestParameterValueParameterWithObjectTypeTest
	{
		public void Check()
		{
    Assert.IsNull(MyAspectAttribute.ParameterValue);
    var classToWeave_L = new ClassToWeave("value");
    Assert.AreEqual("OtherValue", MyAspectAttribute.ParameterValue);
}		


		
				

	}

	
		

		public class MyAspectAttribute : Attribute
{
    public static object ParameterValue;
    public bool NetAspectAttribute = true;
    public void AfterConstructorForParameter(object parameterValue)
    {
        ParameterValue = parameterValue;
    }
}

		public class ClassToWeave
{
    public ClassToWeave([MyAspect] string p)
    {
        p = "OtherValue";
    }
}

}