
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsPart6Sample1BeforeInstructionCallMethodPossibilityTest
{

	[TestFixture]
	public class TestPart6Sample1BeforeInstructionCallMethodPossibilityTest
	{
		public void Check()
		{
    var user = new MyIntUser();
    Assert.AreEqual("Result : 2", user.Compute(12, 6, "Result : {0}"));
    Assert.True(LogAttribute.Called);
}		


		
				

	}

	
		public class MyIntUser
{
    public string Compute(int value, int dividend, string format)
    {
        var myInt = new MyInt(value);
        int result = myInt.DivideBy(dividend);
        return string.Format(format, result);
    }
}

		public class LogAttribute : Attribute
{
    public static bool Called;
    public bool NetAspectAttribute = true;
    public void BeforeCallMethod(MyIntUser caller, MyInt instance, int columnNumber, int lineNumber, string fileName, string filePath, object[] callerParameters, object[] parameters, int v, MethodBase callerMethod, MethodInfo method)
    {
        Called = true;
        Assert.NotNull(caller);
        Assert.NotNull(caller);
        Assert.AreEqual(13, columnNumber);
        Assert.AreEqual(32, lineNumber);
        Assert.AreEqual("Part6Sample1BeforeInstructionCallMethodPossibilityTest.cs", fileName);
        Assert.AreEqual(fileName, Path.GetFileName(filePath));
        Assert.AreEqual(3, callerParameters.Length);
        Assert.AreEqual(1, parameters.Length);
        Assert.AreEqual("Compute", callerMethod.Name);
        Assert.AreEqual("DivideBy", method.Name);
        Assert.AreEqual(6, v);
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
    public int DivideBy(int v)
    {
        return value / v;
    }
}

}