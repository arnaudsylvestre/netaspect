
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsResultParameterWithRealTypeTest
{

	[TestFixture]
	public class TestResultParameterWithRealTypeTest
	{
		public void Check()
		{
    var myInt = new MyInt(24);
    Assert.AreEqual(2, myInt.DivideBy(12));
    Assert.AreEqual(2, LogAttribute.Result);
}		


		
				

	}

	
		

		public class LogAttribute : Attribute
{
    public static int Result;
    public bool NetAspectAttribute = true;
    public void AfterMethod(int result)
    {
        Result = result;
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