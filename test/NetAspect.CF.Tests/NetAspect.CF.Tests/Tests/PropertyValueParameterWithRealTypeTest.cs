
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsPropertyValueParameterWithRealTypeTest
{

	[TestFixture]
	public class TestPropertyValueParameterWithRealTypeTest
	{
		public void Check()
		{
    Assert.AreEqual(0, MyAspectAttribute.I);
    var classToWeave_L = new ClassToWeave();
    classToWeave_L.Weaved = 12;
    Assert.AreEqual(12, MyAspectAttribute.I);
}		


		
				

	}

	
		

		public class MyAspectAttribute : Attribute
{
    public static int I;
    public bool NetAspectAttribute = true;
    public void AfterPropertySetMethod(int propertyValue)
    {
        I = propertyValue;
    }
}

		public class ClassToWeave
{
    [MyAspect]
    public int Weaved
    {
        set
        {
        }
    }
}

}