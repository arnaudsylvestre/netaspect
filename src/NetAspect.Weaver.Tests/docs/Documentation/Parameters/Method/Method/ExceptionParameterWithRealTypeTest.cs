using System;
using System.Reflection;
using NUnit.Framework;
using NetAspect.Weaver.Tests.unit;

namespace NetAspect.Weaver.Tests.docs.Documentation.Parameters.Method.Method
{
   public class ExceptionParameterWithRealTypeTest : NetAspectTest<ExceptionParameterWithRealTypeTest.MyInt>
   {
      public ExceptionParameterWithRealTypeTest()
           : base("It must be of System.Exception type", "MethodWeavingOnException", "MethodWeaving")
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
            try
            {
               var myInt = new MyInt(24);
               myInt.DivideBy(0);
               Assert.Fail("Must raise an exception");
            }
            catch (DivideByZeroException)
            {
            }
            Assert.AreEqual("DivideByZeroException", LogAttribute.Exception.GetType().Name);
         };
      }


      public class LogAttribute : Attribute
      {
          public static Exception Exception;
         public bool NetAspectAttribute = true;

         public void OnExceptionMethod(Exception exception)
         {
             Exception = exception;
         }
      }
   }
}
