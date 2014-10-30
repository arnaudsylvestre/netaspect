
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsExceptionParameterWithRealTypeTest
{

	[TestFixture]
	public class TestExceptionParameterWithRealTypeTest
	{
		public void Check()
		{
    try
    {
        var myInt = new MyInt(24);
        myInt.DivideBy(0);
        Assert.Fail("Must raise an exception");
    }
    catch (DivideByZeroException)
    {
    }
    Assert.AreEqual("DivideByZeroException", LogAttribute.Exception.GetType().Name);
}		


		
				

	}

	
		

		public class LogAttribute : Attribute
{
    public static Exception Exception;
    public bool NetAspectAttribute = true;
    public void OnExceptionMethod(Exception exception)
    {
        Exception = exception;
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