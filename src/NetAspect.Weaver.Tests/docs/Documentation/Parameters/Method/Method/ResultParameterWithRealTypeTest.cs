using System;
using System.Reflection;
using NUnit.Framework;
using NetAspect.Weaver.Tests.unit;

namespace NetAspect.Weaver.Tests.docs.Documentation.Parameters.Method.Method
{
   public class ResultParameterWithRealTypeTest : NetAspectTest<ResultParameterWithRealTypeTest.MyInt>
   {
      public ResultParameterWithRealTypeTest()
           : base("It must be decalred with the same type as the return type of the weaved method", "MethodWeavingAfter", "MethodWeaving")
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
            Assert.AreEqual(2, LogAttribute.Result);
         };
      }


      public class LogAttribute : Attribute
      {
          public static int Result;
         public bool NetAspectAttribute = true;

         public void AfterMethod(int result)
         {
             Result = result;
         }
      }
   }
}
