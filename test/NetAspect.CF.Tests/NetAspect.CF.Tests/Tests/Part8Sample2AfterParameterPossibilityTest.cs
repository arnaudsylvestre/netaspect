
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsPart8Sample2AfterParameterPossibilityTest
{

	[TestFixture]
	public class TestPart8Sample2AfterParameterPossibilityTest
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
    public void AfterMethodForParameter(int parameterValue, MyInt instance, object[] parameters, ParameterInfo parameter, MethodInfo method, int lineNumber, int columnNumber, string fileName, string filePath)
    {
        Called = true;
        Assert.NotNull(instance);
        Assert.AreEqual("v", parameter.Name);
        Assert.AreEqual(1, parameters.Length);
        Assert.AreEqual(6, parameterValue);
        Assert.AreEqual("DivideBy", method.Name);
        Assert.AreEqual(36, lineNumber);
        Assert.AreEqual(10, columnNumber);
        Assert.AreEqual("Part8Sample2AfterParameterPossibilityTest.cs", fileName);
        Assert.True(filePath.EndsWith(@"ParameterWeaving\Part8Sample2AfterParameterPossibilityTest.cs"));
    }
}

		public class MyInt
{
    private readonly int value;
    public MyInt(int value)
    {
        this.value = value;
    }
    public int DivideBy([Log] int v)
    {
        return value / v;
    }
}

}