
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsPart1Sample3OnExceptionMethodPossibilityTest
{

	[TestFixture]
	public class TestPart1Sample3OnExceptionMethodPossibilityTest
	{
		public void Check()
		{
    try
    {
        var myInt = new MyInt(24);
        myInt.DivideBy(0);
        Assert.Fail("Must raise an exception");
    }
    catch (DivideByZeroException)
    {
    }
    Assert.True(LogAttribute.Called);
}		


		
				

	}

	
		

		public class LogAttribute : Attribute
{
    public static bool Called;
    public bool NetAspectAttribute = true;
    public void OnExceptionMethod(object instance, MethodInfo method, object[] parameters, int v, Exception exception, int lineNumber, int columnNumber, string fileName, string filePath)
    {
        Called = true;
        Assert.AreEqual(typeof(MyInt), instance.GetType());
        Assert.AreEqual("DivideBy", method.Name);
        Assert.AreEqual(1, parameters.Length);
        Assert.AreEqual(0, v);
        Assert.AreEqual("DivideByZeroException", exception.GetType().Name);
        Assert.AreEqual(31, lineNumber);
        Assert.AreEqual(10, columnNumber);
        Assert.AreEqual("Part1Sample3OnExceptionMethodPossibilityTest.cs", fileName);
        Assert.True(filePath.EndsWith(@"Method\Part1Sample3OnExceptionMethodPossibilityTest.cs"));
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