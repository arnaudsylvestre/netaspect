
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsParameterParameterWithRealTypeTest
{

	[TestFixture]
	public class TestParameterParameterWithRealTypeTest
	{
		public void Check()
		{
    Assert.IsNull(MyAspectAttribute.Parameter);
    var classToWeave_L = new ClassToWeave(0, "value");
    Assert.AreEqual("p", MyAspectAttribute.Parameter.Name);
}		


		
				

	}

	
		

		public class MyAspectAttribute : Attribute
{
    public static ParameterInfo Parameter;
    public bool NetAspectAttribute = true;
    public void AfterConstructorForParameter(ParameterInfo parameter)
    {
        Parameter = parameter;
    }
}

		public class ClassToWeave
{
    public ClassToWeave(int anotherParameter, [MyAspect] string p)
    {
        p = "";
    }
}

}