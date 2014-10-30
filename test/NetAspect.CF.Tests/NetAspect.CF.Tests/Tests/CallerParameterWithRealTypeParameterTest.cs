
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsCallerParameterWithRealTypeParameterTest
{

	[TestFixture]
	public class TestCallerParameterWithRealTypeParameterTest
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
    public static ClassToWeave Caller;
    public bool NetAspectAttribute = true;
    public void BeforeCallMethod(ClassToWeave caller)
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