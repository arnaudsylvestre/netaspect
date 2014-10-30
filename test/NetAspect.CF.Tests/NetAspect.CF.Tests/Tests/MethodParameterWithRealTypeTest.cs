
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsMethodParameterWithRealTypeTest
{

	[TestFixture]
	public class TestMethodParameterWithRealTypeTest
	{
		public void Check()
		{
    var myInt = new MyInt(24);
    Assert.AreEqual(2, myInt.DivideBy(12));
    Assert.AreEqual(myInt.GetType().GetMethod("DivideBy"), LogAttribute.MethodInfo);
}		


		
				

	}

	
		

		public class LogAttribute : Attribute
{
    public static MethodBase MethodInfo;
    public bool NetAspectAttribute = true;
    public void BeforeMethod(MethodInfo method)
    {
        MethodInfo = method;
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