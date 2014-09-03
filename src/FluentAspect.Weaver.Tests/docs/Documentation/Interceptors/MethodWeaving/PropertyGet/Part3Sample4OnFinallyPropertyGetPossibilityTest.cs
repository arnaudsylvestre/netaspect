using System;
using System.Reflection;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.MethodPossibilities.MethodWeaving.PropertyGet
{
   public class Part3Sample4OnFinallyPropertyGetPossibilityTest : NetAspectTest<Part3Sample4OnFinallyPropertyGetPossibilityTest.MyInt>
   {
      public Part3Sample4OnFinallyPropertyGetPossibilityTest()
         : base("On finally property getter weaving possibilities", "PropertyGetWeavingOnFinally", "PropertyGetWeaving")
      {
      }

      public class MyInt
      {
         private readonly int value;

         public MyInt(int value)
         {
            this.value = value;
         }

         [Log]
         public int Value
         {
            get { return value; }
         }

         public int DivideBy(int v)
         {
            return value / v;
         }
      }

      protected override Action CreateEnsure()
      {
         return () =>
         {
            var myInt = new MyInt(24);
            int val = myInt.Value;
            Assert.True(LogAttribute.Called);
         };
      }


      public class LogAttribute : Attribute
      {
         public static bool Called;
         public bool NetAspectAttribute = true;

         public void OnFinallyPropertyGetMethod(object instance, PropertyInfo property)
         {
            Called = true;
            Assert.AreEqual(typeof (MyInt), instance.GetType());
            Assert.AreEqual("Value", property.Name);
         }
      }
   }
}
