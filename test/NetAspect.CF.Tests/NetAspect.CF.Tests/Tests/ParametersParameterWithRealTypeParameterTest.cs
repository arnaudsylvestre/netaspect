
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsParametersParameterWithRealTypeParameterTest
{

	[TestFixture]
	public class TestParametersParameterWithRealTypeParameterTest
	{
		public void Check()
		{
    var myInt = new MyInt(24);
    Assert.AreEqual(2, myInt.DivideBy(12));
    Assert.AreEqual(1, LogAttribute.Parameters.Length);
    Assert.AreEqual(12, LogAttribute.Parameters[0]);
}		


		
				

	}

	
		

		public class LogAttribute : Attribute
{
    public static object[] Parameters;
    public bool NetAspectAttribute = true;
    public void BeforeMethod(object[] parameters)
    {
        Parameters = parameters;
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