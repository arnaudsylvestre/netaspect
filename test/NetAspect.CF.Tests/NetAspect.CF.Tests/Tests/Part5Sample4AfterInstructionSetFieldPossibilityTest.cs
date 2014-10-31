
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsPart5Sample4AfterInstructionSetFieldPossibilityTest
{

	[TestFixture]
	public class TestPart5Sample4AfterInstructionSetFieldPossibilityTest
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
    public void AfterUpdateField(MyInt callerInstance, MyInt instance, int columnNumber, int lineNumber, string fileName, string filePath, object[] callerParameters, MethodBase callerMethod, FieldInfo field, int newFieldValue)
    {
        Called = true;
        Assert.AreEqual(callerInstance, instance);
        Assert.NotNull(callerInstance);
        Assert.AreEqual(13, columnNumber);
        Assert.AreEqual(33, lineNumber);
        Assert.AreEqual("Part5Sample4AfterInstructionSetFieldPossibilityTest.cs", fileName);
        Assert.AreEqual(fileName, Path.GetFileName(filePath));
        Assert.AreEqual(1, callerParameters.Length);
        Assert.AreEqual("UpdateValue", callerMethod.Name);
        Assert.AreEqual("value", field.Name);
        Assert.AreEqual(6, newFieldValue);
    }
}

		public class MyInt
{
    [Log]
    private int value;
    public void UpdateValue(int intValue)
    {
        value = intValue;
    }
    public int DivideBy(int v)
    {
        return value / v;
    }
}

}