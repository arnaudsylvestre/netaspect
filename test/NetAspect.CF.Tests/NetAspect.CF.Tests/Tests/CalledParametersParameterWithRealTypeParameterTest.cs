
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsCalledParametersParameterWithRealTypeParameterTest
{

	[TestFixture]
	public class TestCalledParametersParameterWithRealTypeParameterTest
	{
		public void Check()
		{
    Assert.IsNull(MyAspectAttribute.CalledParameters);
    var classToWeave_L = new ClassToWeave();
    classToWeave_L.Weaved();
    Assert.AreEqual(new object[] {
        1,
        2
    }, MyAspectAttribute.CalledParameters);
}		


		
				

	}

	
		

		public class MyAspectAttribute : Attribute
{
    public static object[] CalledParameters;
    public bool NetAspectAttribute = true;
    public void BeforeCallMethod(object[] parameters)
    {
        CalledParameters = parameters;
    }
}

		public class ClassToWeave
{
    [MyAspect]
    public string Method(int param1, int param2)
    {
        return "Hello";
    }
    public string Weaved()
    {
        return Method(1, 2);
    }
}

}