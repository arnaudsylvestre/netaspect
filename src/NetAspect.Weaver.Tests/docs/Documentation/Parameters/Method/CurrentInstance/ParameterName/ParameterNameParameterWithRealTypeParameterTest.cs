using System;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.Documentation.Parameters.Method.CurrentInstance.ParameterName
{
   public class ParameterNameParameterWithRealTypeParameterTest : NetAspectTest<ParameterNameParameterWithRealTypeParameterTest.MyInt>
   {
      public ParameterNameParameterWithRealTypeParameterTest()
         : base("It must be declared with the same type as the type of the parameter in the weaved method", "MethodWeavingBefore", "MethodWeaving")
      {
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
            get { return value; }
         }

         [Log]
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
            Assert.AreEqual(2, myInt.DivideBy(12));
            Assert.AreEqual(12, LogAttribute.V);
         };
      }


      public class LogAttribute : Attribute
      {
          public static int V;
         public bool NetAspectAttribute = true;

         public void BeforeMethod(int v)
         {
             V = v;
         }
      }
   }
}
