
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsPart2Sample3OnExceptionConstructorPossibilityTest
{

	[TestFixture]
	public class TestPart2Sample3OnExceptionConstructorPossibilityTest
	{
		public void Check()
		{
    try
    {
        new MyInt(0);
        Assert.Fail("Must raise an exception");
    }
    catch (NotSupportedException)
    {
    }
    Assert.True(LogAttribute.Called);
}		


		
				

	}

	
		

		public class LogAttribute : Attribute
{
    public static bool Called;
    public bool NetAspectAttribute = true;
    public void OnExceptionConstructor(object instance, ConstructorInfo constructor, object[] parameters, int intValue, Exception exception, int lineNumber, int columnNumber, string fileName, string filePath)
    {
        Called = true;
        Assert.AreEqual(typeof(MyInt), instance.GetType());
        Assert.AreEqual(".ctor", constructor.Name);
        Assert.AreEqual(1, parameters.Length);
        Assert.AreEqual(0, intValue);
        Assert.AreEqual("NotSupportedException", exception.GetType().Name);
        Assert.AreEqual(19, lineNumber);
        Assert.AreEqual(10, columnNumber);
        Assert.AreEqual("Part2Sample3OnExceptionConstructorPossibilityTest.cs", fileName);
        Assert.True(filePath.EndsWith(@"Constructor\Part2Sample3OnExceptionConstructorPossibilityTest.cs"));
    }
}

		public class MyInt
{
    private readonly int value;
    [Log]
    public MyInt(int intValue)
    {
        if (value == 0)
            throw new NotSupportedException();
        value = intValue;
    }
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