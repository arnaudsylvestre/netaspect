
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsParameterNameParameterWithRealTypeParameterTest
{

	[TestFixture]
	public class TestParameterNameParameterWithRealTypeParameterTest
	{
		public void Check()
		{
    var myInt = new MyInt(24);
    Assert.AreEqual(2, myInt.DivideBy(12));
    Assert.AreEqual(12, LogAttribute.V);
}		


		
				

	}

	
		

		public class LogAttribute : Attribute
{
    public static int V;
    public bool NetAspectAttribute = true;
    public void BeforeMethod(int v)
    {
        V = v;
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