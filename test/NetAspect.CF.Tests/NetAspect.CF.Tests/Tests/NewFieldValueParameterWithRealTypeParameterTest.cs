
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsNewFieldValueParameterWithRealTypeParameterTest
{

	[TestFixture]
	public class TestNewFieldValueParameterWithRealTypeParameterTest
	{
		public void Check()
		{
    var classToWeave_L = new ClassToWeave();
    classToWeave_L.Weaved();
    Assert.AreEqual("Hello", MyAspectAttribute.Value);
}		


		
				

	}

	
		

		public class MyAspectAttribute : Attribute
{
    public static string Value;
    public bool NetAspectAttribute = true;
    public void BeforeUpdateField(string newFieldValue)
    {
        Value = newFieldValue;
    }
}

		public class ClassToWeave
{
    [MyAspect]
    public string field;
    public string Weaved()
    {
        field = "Hello";
        return field;
    }
}

}