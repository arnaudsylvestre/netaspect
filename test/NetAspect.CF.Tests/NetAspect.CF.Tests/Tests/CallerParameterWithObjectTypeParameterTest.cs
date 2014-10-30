
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsCallerParameterWithObjectTypeParameterTest
{

	[TestFixture]
	public class TestCallerParameterWithObjectTypeParameterTest
	{
		public void Check()
		{
    Assert.IsNull(MyAspectAttribute.Caller);
    var classToWeave_L = new ClassToWeave();
    classToWeave_L.Weaved();
    Assert.AreEqual(classToWeave_L, MyAspectAttribute.Caller);
}		


		
				

	}

	
		

		public class MyAspectAttribute : Attribute
{
    public static object Caller;
    public bool NetAspectAttribute = true;
    public void BeforeCallMethod(object caller)
    {
        Caller = caller;
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