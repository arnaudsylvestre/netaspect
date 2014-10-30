
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsGettingStartedPart2Test
{

	[TestFixture]
	public class TestGettingStartedPart2Test
	{
		public void Check()
		{
    var computer = new Computer();
    try
    {
        computer.Divide(12, 0);
    }
    catch (Exception)
    {
        Assert.AreEqual("An exception !", LogAttribute.writer.ToString());
    }
}		


		
				

	}

	
		public class Computer
{
    [Log]
    public int Divide(int a, int b)
    {
        return a / b;
    }
}

		public class LogAttribute : Attribute
{
    public bool NetAspectAttribute = true;
    public static StringWriter writer = new StringWriter();
    public void OnExceptionMethod()
    {
        writer.Write("An exception !");
    }
}

		

}