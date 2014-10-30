
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsPart4Sample3OnExceptionPropertySetPossibilityTest
{

	[TestFixture]
	public class TestPart4Sample3OnExceptionPropertySetPossibilityTest
	{
		public void Check()
		{
    try
    {
        var myInt = new MyInt(0);
        myInt.Value = 0;
        Assert.Fail("Must raise an exception");
    }
    catch (NotSupportedException)
    {
    }
    Assert.True(LogAttribute.Called);
}		


		
				

	}

	
		

		public class LogAttribute : Attribute
{
    public static bool Called;
    public bool NetAspectAttribute = true;
    public void OnExceptionPropertySetMethod(object instance, PropertyInfo property, Exception exception, int lineNumber, int columnNumber, string fileName, string filePath)
    {
        Called = true;
        Assert.AreEqual(typeof(MyInt), instance.GetType());
        Assert.AreEqual("Value", property.Name);
        Assert.AreEqual("NotSupportedException", exception.GetType().Name);
        Assert.AreEqual(28, lineNumber);
        Assert.AreEqual(13, columnNumber);
        Assert.AreEqual("Part4Sample3OnExceptionPropertySetPossibilityTest.cs", fileName);
        Assert.True(filePath.EndsWith(@"PropertySet\Part4Sample3OnExceptionPropertySetPossibilityTest.cs"));
    }
}

		public class MyInt
{
    private int value;
    public MyInt(int value)
    {
        this.value = value;
    }
    [Log]
    public int Value
    {
        set
        {
            if (value == 0)
                throw new NotSupportedException("Must not be 0");
            this.value = value;
        }
    }
    public int DivideBy(int v)
    {
        return value / v;
    }
}

}