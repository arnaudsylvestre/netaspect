
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsCalledParameterNameParameterWithRealTypeParameterTest
{

	[TestFixture]
	public class TestCalledParameterNameParameterWithRealTypeParameterTest
	{
		public void Check()
		{
    Assert.AreEqual(0, MyAspectAttribute.Value);
    var classToWeave_L = new ClassToWeave();
    classToWeave_L.Weaved();
    Assert.AreEqual(12, MyAspectAttribute.Value);
}		


		
				

	}

	
		

		public class MyAspectAttribute : Attribute
{
    public static int Value;
    public bool NetAspectAttribute = true;
    public void BeforeCallMethod(int param1)
    {
        Value = param1;
    }
}

		public class ClassToWeave
{
    [MyAspect]
    public string Method(int param1)
    {
        return "Hello";
    }
    public string Weaved()
    {
        return Method(12);
    }
}

}