
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsPart7Sample3BeforeInstructionSetPropertyPossibilityTest
{

	[TestFixture]
	public class TestPart7Sample3BeforeInstructionSetPropertyPossibilityTest
	{
		public void Check()
		{
    var classToWeave_L = new MyInt();
    classToWeave_L.UpdateValue(6);
    Assert.True(LogAttribute.Called);
}		


		
				

	}

	
		

		public class LogAttribute : Attribute
{
    public static bool Called;
    public bool NetAspectAttribute = true;
    public void BeforeUpdateProperty(MyInt caller, MyInt instance, int columnNumber, int lineNumber, string fileName, string filePath, object[] callerParameters, MethodBase callerMethod, PropertyInfo property, int newPropertyValue)
    {
        Called = true;
        Assert.AreEqual(caller, instance);
        Assert.NotNull(caller);
        Assert.AreEqual(13, columnNumber);
        Assert.AreEqual(34, lineNumber);
        Assert.AreEqual("Part7Sample3BeforeInstructionSetPropertyPossibilityTest.cs", fileName);
        Assert.AreEqual(fileName, Path.GetFileName(filePath));
        Assert.AreEqual(1, callerParameters.Length);
        Assert.AreEqual("UpdateValue", callerMethod.Name);
        Assert.AreEqual("Value", property.Name);
        Assert.AreEqual(6, newPropertyValue);
    }
}

		public class MyInt
{
    [Log]
    private int Value
    {
        get;
        set;
    }
    public void UpdateValue(int intValue)
    {
        Value = intValue;
    }
    public int DivideBy(int v)
    {
        return Value / v;
    }
}

}