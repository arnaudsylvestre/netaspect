
using System;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace NetAspect.nsPerTypeLifeCycleSampleTest
{

	[TestFixture]
	public class TestPerTypeLifeCycleSampleTest
	{
		public void Check()
		{
    var classToWeave = new ClassToWeave();
    classToWeave.Weaved();
    Assert.AreEqual(1, MyAspectAttribute.i);
    classToWeave.Weaved();
    Assert.AreEqual(2, MyAspectAttribute.i);
    var otherClassToWeave = new ClassToWeave();
    otherClassToWeave.Weaved();
    Assert.AreEqual(3, MyAspectAttribute.i);
}		


		
				

	}

	
		

		public class MyAspectAttribute : Attribute
{
    public static int i;
    public static string LifeCycle = "PerType";
    public bool NetAspectAttribute = true;
    public MyAspectAttribute()
    {
        i = 0;
    }
    public void BeforeMethod()
    {
        i++;
    }
}

		public class ClassToWeave
{
    [MyAspect]
    public void Weaved()
    {
    }
}

}