
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsPart3Sample4OnFinallyPropertyGetPossibilityTest
{

	[TestFixture]
	public class TestPart3Sample4OnFinallyPropertyGetPossibilityTest
	{
		public void Check()
		{
    var myInt = new MyInt(24);
    int val = myInt.Value;
    Assert.True(LogAttribute.Called);
}		


		
				

	}

	
		

		public class LogAttribute : Attribute
{
    public static bool Called;
    public bool NetAspectAttribute = true;
    public void OnFinallyPropertyGetMethod(object instance, PropertyInfo property, int lineNumber, int columnNumber, string fileName, string filePath)
    {
        Called = true;
        Assert.AreEqual(typeof(MyInt), instance.GetType());
        Assert.AreEqual("Value", property.Name);
        Assert.AreEqual(27, lineNumber);
        Assert.AreEqual(17, columnNumber);
        Assert.AreEqual("Part3Sample4OnFinallyPropertyGetPossibilityTest.cs", fileName);
        Assert.True(filePath.EndsWith(@"PropertyGet\Part3Sample4OnFinallyPropertyGetPossibilityTest.cs"));
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