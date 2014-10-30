
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsPart3Sample1BeforePropertyGetPossibilityTest
{

	[TestFixture]
	public class TestPart3Sample1BeforePropertyGetPossibilityTest
	{
		public void Check()
		{
    var myInt = new MyInt(24);
    Assert.AreEqual(24, myInt.Value);
    Assert.True(LogAttribute.Called);
}		


		
				

	}

	
		

		public class LogAttribute : Attribute
{
    public static bool Called;
    public bool NetAspectAttribute = true;
    public void BeforePropertyGetMethod(object instance, PropertyInfo property, int lineNumber, int columnNumber, string fileName, string filePath)
    {
        Called = true;
        Assert.AreEqual(typeof(MyInt), instance.GetType());
        Assert.AreEqual("Value", property.Name);
        Assert.AreEqual(28, lineNumber);
        Assert.AreEqual(17, columnNumber);
        Assert.AreEqual("Part3Sample1BeforePropertyGetPossibilityTest.cs", fileName);
        Assert.True(filePath.EndsWith(@"PropertyGet\Part3Sample1BeforePropertyGetPossibilityTest.cs"));
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