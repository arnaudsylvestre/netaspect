using System;
using System.Reflection;
using NUnit.Framework;
using NetAspect.Weaver.Tests.unit;

namespace NetAspect.Weaver.Tests.docs.Documentation.Parameters.Method.CurrentMember
{
   public class PropertyParameterWithRealTypeTest : NetAspectTest<PropertyParameterWithRealTypeTest.MyInt>
   {
      public PropertyParameterWithRealTypeTest()
           : base("It must be of System.Reflection.PropertyInfo type", "PropertyGetWeavingBefore", "PropertyGetWeaving")
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
            Assert.AreEqual(24, myInt.Value);
            Assert.AreEqual("Value", LogAttribute.Property.Name);
         };
      }


      public class LogAttribute : Attribute
      {
          public static PropertyInfo Property;
         public bool NetAspectAttribute = true;

         public void BeforePropertyGetMethod(PropertyInfo property)
         {
             Property = property;
         }
      }
   }
}
