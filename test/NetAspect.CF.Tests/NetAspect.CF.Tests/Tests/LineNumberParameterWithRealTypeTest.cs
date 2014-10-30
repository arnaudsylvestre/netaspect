
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsLineNumberParameterWithRealTypeTest
{

	[TestFixture]
	public class TestLineNumberParameterWithRealTypeTest
	{
		public void Check()
		{
    Computer.Divide(6, 3);
    Assert.AreEqual(50, LogAttribute.LineNumber);
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
    public static int LineNumber;
    public bool NetAspectAttribute = true;
    public void BeforeCallMethod(int lineNumber)
    {
        LineNumber = lineNumber;
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