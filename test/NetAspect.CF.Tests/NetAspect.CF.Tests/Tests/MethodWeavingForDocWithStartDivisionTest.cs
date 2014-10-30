
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsMethodWeavingForDocWithStartDivisionTest
{

	[TestFixture]
	public class TestMethodWeavingForDocWithStartDivisionTest
	{
		public void Check()
		{
    LogAttribute.Console = new StringBuilder();
    var value = new MyInt(6);
    value.DivideBy(3);
    Assert.AreEqual("Start Division", LogAttribute.Console.ToString());
}		


		
				

	}

	
		

		public class LogAttribute : Attribute
{
    public static StringBuilder Console;
    public bool NetAspectAttribute = true;
    public void BeforeMethod()
    {
        Console.Append("Start Division");
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