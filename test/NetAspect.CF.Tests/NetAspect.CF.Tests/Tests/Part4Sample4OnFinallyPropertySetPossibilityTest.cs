
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsPart4Sample4OnFinallyPropertySetPossibilityTest
{

	[TestFixture]
	public class TestPart4Sample4OnFinallyPropertySetPossibilityTest
	{
		public void Check()
		{
    var myInt = new MyInt(24);
    myInt.Value = 12;
    Assert.True(LogAttribute.Called);
}		


		
				

	}

	
		

		public class LogAttribute : Attribute
{
    public static bool Called;
    public bool NetAspectAttribute = true;
    public void OnFinallyPropertySetMethod(object instance, PropertyInfo property, int propertyValue, int lineNumber, int columnNumber, string fileName, string filePath)
    {
        Called = true;
        Assert.AreEqual(typeof(MyInt), instance.GetType());
        Assert.AreEqual("Value", property.Name);
        Assert.AreEqual(12, propertyValue);
        Assert.AreEqual(27, lineNumber);
        Assert.AreEqual(17, columnNumber);
        Assert.AreEqual("Part4Sample4OnFinallyPropertySetPossibilityTest.cs", fileName);
        Assert.True(filePath.EndsWith(@"PropertySet\Part4Sample4OnFinallyPropertySetPossibilityTest.cs"));
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