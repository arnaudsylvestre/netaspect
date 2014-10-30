
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsPart5Sample1BeforeInstructionGetFieldPossibilityTest
{

	[TestFixture]
	public class TestPart5Sample1BeforeInstructionGetFieldPossibilityTest
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
    public void BeforeGetField(MyInt caller, MyInt instance, int columnNumber, int lineNumber, string fileName, string filePath, object[] callerParameters, MethodBase callerMethod, FieldInfo field)
    {
        Called = true;
        Assert.AreEqual(caller, instance);
        Assert.NotNull(caller);
        Assert.AreEqual(13, columnNumber);
        Assert.AreEqual(38, lineNumber);
        Assert.AreEqual("Part5Sample1BeforeInstructionGetFieldPossibilityTest.cs", fileName);
        Assert.AreEqual(fileName, Path.GetFileName(filePath));
        Assert.AreEqual(1, callerParameters.Length);
        Assert.AreEqual("DivideBy", callerMethod.Name);
        Assert.AreEqual("value", field.Name);
    }
}

		public class MyInt
{
    [Log]
    private readonly int value;
    public MyInt(int value)
    {
        this.value = value;
    }
    public int DivideBy(int v)
    {
        return value / v;
    }
}

}