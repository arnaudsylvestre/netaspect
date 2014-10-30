
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsPart1Sample1BeforeMethodPossibilityTest
{

	[TestFixture]
	public class TestPart1Sample1BeforeMethodPossibilityTest
	{
		public void Check()
		{
    var myInt = new MyInt(24);
    Assert.AreEqual(2, myInt.DivideBy(12));
    Assert.True(LogAttribute.Called);
}		


		
				

	}

	
		

		public class LogAttribute : Attribute
{
    public static bool Called;
    public bool NetAspectAttribute = true;
    public void BeforeMethod(object instance, MethodInfo method, object[] parameters, int v, int lineNumber, int columnNumber, string fileName, string filePath)
    {
        Called = true;
        Assert.AreEqual(typeof(MyInt), instance.GetType());
        Assert.AreEqual("DivideBy", method.Name);
        Assert.AreEqual(1, parameters.Length);
        Assert.AreEqual(12, v);
        Assert.AreEqual(32, lineNumber);
        Assert.AreEqual(10, columnNumber);
        Assert.AreEqual("Part1Sample1BeforeMethodPossibilityTest.cs", fileName);
        Assert.True(filePath.EndsWith(@"Method\Part1Sample1BeforeMethodPossibilityTest.cs"));
    }
}

		public class MyInt
{
    private readonly int value;
    public MyInt(int value)
    {
        this.value = value;
    }
    public int Value
    {
        get
        {
            return value;
        }
    }
    [Log]
    public int DivideBy(int v)
    {
        return value / v;
    }
}

}