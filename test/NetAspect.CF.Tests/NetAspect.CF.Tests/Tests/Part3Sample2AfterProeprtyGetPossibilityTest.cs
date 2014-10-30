
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsPart3Sample2AfterProeprtyGetPossibilityTest
{

	[TestFixture]
	public class TestPart3Sample2AfterProeprtyGetPossibilityTest
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
    public void AfterPropertyGetMethod(object instance, PropertyInfo property, int result, int lineNumber, int columnNumber, string fileName, string filePath)
    {
        Called = true;
        Assert.AreEqual(typeof(MyInt), instance.GetType());
        Assert.AreEqual("Value", property.Name);
        Assert.AreEqual(24, result);
        Assert.AreEqual(27, lineNumber);
        Assert.AreEqual(17, columnNumber);
        Assert.AreEqual("Part3Sample2AfterProeprtyGetPossibilityTest.cs", fileName);
        Assert.True(filePath.EndsWith(@"PropertyGet\Part3Sample2AfterProeprtyGetPossibilityTest.cs"));
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