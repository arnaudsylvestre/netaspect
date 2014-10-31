
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsPart7Sample2AfterInstructionGetPropertyPossibilityTest
{

	[TestFixture]
	public class TestPart7Sample2AfterInstructionGetPropertyPossibilityTest
	{
		public void Check()
		{
    var classToWeave_L = new MyInt(12);
    classToWeave_L.DivideBy(6);
    Assert.True(LogAttribute.Called);
}		


		
				

	}

	
		

		public class LogAttribute : Attribute
{
    public static bool Called;
    public bool NetAspectAttribute = true;
    public void AfterGetProperty(MyInt callerInstance, MyInt instance, int columnNumber, int lineNumber, string fileName, string filePath, object[] callerParameters, MethodBase callerMethod, PropertyInfo property, int result)
    {
        Called = true;
        Assert.AreEqual(callerInstance, instance);
        Assert.NotNull(callerInstance);
        Assert.AreEqual(13, columnNumber);
        Assert.AreEqual(39, lineNumber);
        Assert.AreEqual("Part7Sample2AfterInstructionGetPropertyPossibilityTest.cs", fileName);
        Assert.AreEqual(fileName, Path.GetFileName(filePath));
        Assert.AreEqual(1, callerParameters.Length);
        Assert.AreEqual("DivideBy", callerMethod.Name);
        Assert.AreEqual("Value", property.Name);
        Assert.AreEqual(12, result);
    }
}

		public class MyInt
{
    public MyInt(int value)
    {
        Value = value;
    }
    [Log]
    private int Value
    {
        get;
        set;
    }
    public int DivideBy(int v)
    {
        return Value / v;
    }
}

}