
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsPart2Sample2AfterConstructorPossibilityTest
{

	[TestFixture]
	public class TestPart2Sample2AfterConstructorPossibilityTest
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
    public void AfterConstructor(object instance, ConstructorInfo constructor, object[] parameters, int intValue, int lineNumber, int columnNumber, string fileName, string filePath)
    {
        Called = true;
        Assert.AreEqual(typeof(MyInt), instance.GetType());
        Assert.AreEqual(".ctor", constructor.Name);
        Assert.AreEqual(1, parameters.Length);
        Assert.AreEqual(24, intValue);
        Assert.AreEqual(19, lineNumber);
        Assert.AreEqual(10, columnNumber);
        Assert.AreEqual("Part2Sample2AfterConstructorPossibilityTest.cs", fileName);
        Assert.True(filePath.EndsWith(@"Constructor\Part2Sample2AfterConstructorPossibilityTest.cs"));
    }
}

		public class MyInt
{
    private readonly int value;
    [Log]
    public MyInt(int intValue)
    {
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