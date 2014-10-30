
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsColumnNumberParameterWithRealTypeTest
{

	[TestFixture]
	public class TestColumnNumberParameterWithRealTypeTest
	{
		public void Check()
		{
    Computer.Divide(6, 3);
    Assert.AreEqual(13, LogAttribute.ColumnNumber);
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
    public static int ColumnNumber;
    public bool NetAspectAttribute = true;
    public void BeforeCallMethod(int columnNumber)
    {
        ColumnNumber = columnNumber;
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