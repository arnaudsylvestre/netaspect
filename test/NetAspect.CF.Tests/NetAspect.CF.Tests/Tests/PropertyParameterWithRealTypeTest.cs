
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsPropertyParameterWithRealTypeTest
{

	[TestFixture]
	public class TestPropertyParameterWithRealTypeTest
	{
		public void Check()
		{
    var myInt = new MyInt(24);
    Assert.AreEqual(24, myInt.Value);
    Assert.AreEqual("Value", LogAttribute.Property.Name);
}		


		
				

	}

	
		

		public class LogAttribute : Attribute
{
    public static PropertyInfo Property;
    public bool NetAspectAttribute = true;
    public void BeforePropertyGetMethod(PropertyInfo property)
    {
        Property = property;
    }
}

		public class MyInt
{
    private readonly int value;
    public MyInt(int value)
    {
        this.value = value;
    }
    [Log]
    public int Value
    {
        get
        {
            return value;
        }
    }
    public int DivideBy(int v)
    {
        return value / v;
    }
}

}