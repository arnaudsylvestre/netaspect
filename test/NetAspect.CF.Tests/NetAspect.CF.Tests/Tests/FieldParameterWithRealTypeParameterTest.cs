
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsFieldParameterWithRealTypeParameterTest
{

	[TestFixture]
	public class TestFieldParameterWithRealTypeParameterTest
	{
		public void Check()
		{
    var classToWeave_L = new MyInt(12);
    classToWeave_L.DivideBy(6);
    Assert.AreEqual("value", LogAttribute.Field.Name);
}		


		
				

	}

	
		

		public class LogAttribute : Attribute
{
    public static FieldInfo Field;
    public bool NetAspectAttribute = true;
    public void BeforeGetField(FieldInfo field)
    {
        Field = field;
    }
}

		public class MyInt
{
    [Log]
    private readonly int value;
    public MyInt(int value)
    {
        this.value = value;
    }
    public int DivideBy(int v)
    {
        return value / v;
    }
}

}