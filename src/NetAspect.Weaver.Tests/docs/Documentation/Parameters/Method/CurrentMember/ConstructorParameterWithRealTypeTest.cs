using System;
using System.Reflection;
using NUnit.Framework;
using NetAspect.Weaver.Tests.unit;

namespace NetAspect.Weaver.Tests.docs.Documentation.Parameters.Method.CurrentMember
{
    public class ConstructorParameterWithRealTypeTest : NetAspectTest<ConstructorParameterWithRealTypeTest.MyInt>
   {
      public ConstructorParameterWithRealTypeTest()
            : base("It must be of System.Reflection.MethodBase type", "ConstructorWeavingBefore", "ConstructorWeaving")
      {
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
            Assert.AreEqual(".ctor", LogAttribute.Constructor.Name);
         };
      }


      public class LogAttribute : Attribute
      {
          public static MethodBase Constructor;
         public bool NetAspectAttribute = true;

         public void BeforeConstructor(MethodBase constructor)
         {
             Constructor = constructor;
            
         }
      }
   }
}
