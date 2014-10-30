
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsFileNameParameterWithRealTypeTest
{

	[TestFixture]
	public class TestFileNameParameterWithRealTypeTest
	{
		public void Check()
		{
    Computer.Divide(6, 3);
    Assert.AreEqual("FileNameParameterWithRealTypeTest.cs", LogAttribute.FileName);
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
    public static string FileName;
    public bool NetAspectAttribute = true;
    public void BeforeCallMethod(string fileName)
    {
        FileName = fileName;
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