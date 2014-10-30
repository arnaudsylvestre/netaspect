
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsFilePathParameterWithRealTypeTest
{

	[TestFixture]
	public class TestFilePathParameterWithRealTypeTest
	{
		public void Check()
		{
    Computer.Divide(6, 3);
    Assert.True(LogAttribute.FilePath.EndsWith(@"FilePath\FilePathParameterWithRealTypeTest.cs"));
}		


		
				

	}

	
		public class Computer
{
    public static int Divide(int a, int b)
    {
        var myInt = new MyInt(a);
        return myInt.DivideBy(b);
    }
}

		public class LogAttribute : Attribute
{
    public static string FilePath;
    public bool NetAspectAttribute = true;
    public void BeforeCallMethod(string filePath)
    {
        FilePath = filePath;
    }
}

		public class MyInt
{
    private readonly int value;
    public MyInt(int value)
    {
        this.value = value;
    }
    public int Value
    {
        get
        {
            return value;
        }
    }
    [Log]
    public int DivideBy(int v)
    {
        return value / v;
    }
}

}