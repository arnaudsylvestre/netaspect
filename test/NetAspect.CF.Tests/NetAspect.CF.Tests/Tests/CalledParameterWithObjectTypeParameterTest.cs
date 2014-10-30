
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsCalledParameterWithObjectTypeParameterTest
{

	[TestFixture]
	public class TestCalledParameterWithObjectTypeParameterTest
	{
		public void Check()
		{
    Assert.IsNull(MyAspectAttribute.Instance);
    var classToWeave_L = new ClassToWeave();
    classToWeave_L.Weaved();
    Assert.AreEqual(classToWeave_L, MyAspectAttribute.Instance);
}		


		
				

	}

	
		

		public class MyAspectAttribute : Attribute
{
    public static object Instance;
    public bool NetAspectAttribute = true;
    public void BeforeCallMethod(object instance)
    {
        Instance = instance;
    }
}

		public class ClassToWeave
{
    [MyAspect]
    public string Method()
    {
        return "Hello";
    }
    public string Weaved()
    {
        return Method();
    }
}

}