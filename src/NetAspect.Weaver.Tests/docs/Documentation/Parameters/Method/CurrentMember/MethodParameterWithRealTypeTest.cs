using System;
using System.Reflection;
using NetAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.docs.Documentation.Parameters
{
   public class MethodParameterWithRealTypeTest : NetAspectTest<MethodParameterWithRealTypeTest.MyInt>
   {
      public MethodParameterWithRealTypeTest()
         : base("It must be of System.Reflection.MethodBase type", "MethodWeavingBefore", "MethodWeaving")
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
            Assert.AreEqual(myInt.GetType().GetMethod("DivideBy"), LogAttribute.MethodInfo);
         };
      }


      public class LogAttribute : Attribute
      {
         public static MethodBase MethodInfo;
         public bool NetAspectAttribute = true;

         public void BeforeMethod(MethodInfo method)
         {
            MethodInfo = method;
         }
      }
   }
}
