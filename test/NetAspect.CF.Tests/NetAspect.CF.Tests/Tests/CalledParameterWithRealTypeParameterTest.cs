
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsCalledParameterWithRealTypeParameterTest
{

	[TestFixture]
	public class TestCalledParameterWithRealTypeParameterTest
	{
		public void Check()
		{
    Assert.IsNull(MyAspectAttribute.Called);
    var classToWeave_L = new ClassToWeave();
    classToWeave_L.Weaved();
    Assert.AreEqual(classToWeave_L, MyAspectAttribute.Called);
}		


		
				

	}

	
		

		public class MyAspectAttribute : Attribute
{
    public static ClassToWeave Called;
    public bool NetAspectAttribute = true;
    public void BeforeCallMethod(ClassToWeave instance)
    {
        Called = instance;
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