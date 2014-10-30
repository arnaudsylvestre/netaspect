
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsPart8Sample2AfterInstructionCallMethodPossibilityTest
{

	[TestFixture]
	public class TestPart8Sample2AfterInstructionCallMethodPossibilityTest
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
    public void AfterCallConstructor(MyIntUser caller, int columnNumber, int lineNumber, string fileName, string filePath, object[] callerParameters, object[] parameters, int value, MethodBase callerMethod, ConstructorInfo constructor)
    {
        Called = true;
        Assert.NotNull(caller);
        Assert.NotNull(constructor);
        Assert.AreEqual(13, columnNumber);
        Assert.AreEqual(31, lineNumber);
        Assert.AreEqual("Part8Sample2AfterInstructionCallMethodPossibilityTest.cs", fileName);
        Assert.AreEqual(fileName, Path.GetFileName(filePath));
        Assert.AreEqual(3, callerParameters.Length);
        Assert.AreEqual(1, parameters.Length);
        Assert.AreEqual("Compute", callerMethod.Name);
        Assert.AreEqual(12, value);
    }
}

		public class MyInt
{
    private readonly int value;
    [Log]
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