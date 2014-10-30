
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsPart4Sample1BeforePropertySetPossibilityTest
{

	[TestFixture]
	public class TestPart4Sample1BeforePropertySetPossibilityTest
	{
		public void Check()
		{
    var myInt = new MyInt(24);
    myInt.Value = 32;
    Assert.True(LogAttribute.Called);
}		


		
				

	}

	
		

		public class LogAttribute : Attribute
{
    public static bool Called;
    public bool NetAspectAttribute = true;
    public void BeforePropertySetMethod(object instance, PropertyInfo property, int propertyValue, int lineNumber, int columnNumber, string fileName, string filePath)
    {
        Called = true;
        Assert.AreEqual(typeof(MyInt), instance.GetType());
        Assert.AreEqual("Value", property.Name);
        Assert.AreEqual(32, propertyValue);
        Assert.AreEqual(28, lineNumber);
        Assert.AreEqual(17, columnNumber);
        Assert.AreEqual("Part4Sample1BeforePropertySetPossibilityTest.cs", fileName);
        Assert.True(filePath.EndsWith(@"PropertySet\Part4Sample1BeforePropertySetPossibilityTest.cs"));
    }
}

		public class MyInt
{
    private int value;
    public MyInt(int value)
    {
        this.value = value;
    }
    [Log]
    public int Value
    {
        set
        {
            this.value = value;
        }
    }
    public int DivideBy(int v)
    {
        return value / v;
    }
}

}