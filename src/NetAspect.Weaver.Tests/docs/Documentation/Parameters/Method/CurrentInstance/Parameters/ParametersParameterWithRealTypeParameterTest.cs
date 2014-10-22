using System;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.Documentation.Parameters.Method.CurrentInstance.Parameters
{
   public class ParametersParameterWithRealTypeParameterTest : NetAspectTest<ParametersParameterWithRealTypeParameterTest.MyInt>
   {
      public ParametersParameterWithRealTypeParameterTest()
         : base("It must be declared with the System.Object[] type", "MethodWeavingBefore", "MethodWeaving")
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
            Assert.AreEqual(1, LogAttribute.Parameters.Length);
            Assert.AreEqual(12, LogAttribute.Parameters[0]);
         };
      }


      public class LogAttribute : Attribute
      {
          public static object[] Parameters;
         public bool NetAspectAttribute = true;

         public void BeforeMethod(object[] parameters)
         {
             Parameters = parameters;
         }
      }
   }
}
