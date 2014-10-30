
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsInstanceParameterWithObjectTypeParameterTest
{

	[TestFixture]
	public class TestInstanceParameterWithObjectTypeParameterTest
	{
		public void Check()
		{
    var myInt = new MyInt(24);
    Assert.AreEqual(2, myInt.DivideBy(12));
    Assert.AreSame(myInt, LogAttribute.Instance);
}		


		
				

	}

	
		

		public class LogAttribute : Attribute
{
    public static object Instance;
    public bool NetAspectAttribute = true;
    public void BeforeMethod(object instance)
    {
        Instance = instance;
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