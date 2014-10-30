
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsConstructorParameterWithRealTypeTest
{

	[TestFixture]
	public class TestConstructorParameterWithRealTypeTest
	{
		public void Check()
		{
    var myInt = new MyInt(24);
    Assert.AreEqual(".ctor", LogAttribute.Constructor.Name);
}		


		
				

	}

	
		

		public class LogAttribute : Attribute
{
    public static MethodBase Constructor;
    public bool NetAspectAttribute = true;
    public void BeforeConstructor(ConstructorInfo constructor)
    {
        Constructor = constructor;
    }
}

		public class MyInt
{
    private readonly int value;
    [Log]
    public MyInt(int intValue)
    {
        value = intValue;
    }
    public int Value
    {
        get
        {
            return value;
        }
    }
    public int DivideBy(int v)
    {
        return value / v;
    }
}

}