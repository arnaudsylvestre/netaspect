using System;
using NUnit.Framework;
using NetAspect.Weaver.Tests.unit;

namespace NetAspect.Weaver.Tests.docs.Documentation.Parameters.Pdb.FilePath
{
   public class FilePathParameterWithRealTypeTest : NetAspectTest<FilePathParameterWithRealTypeTest.MyInt>
   {
       public FilePathParameterWithRealTypeTest()
           : base("It must be declared with the System.String type", "MethodWeavingBefore", "MethodWeaving")
       {
           
       }

      protected override Action CreateEnsure()
      {
         return () =>
         {
            Computer.Divide(6, 3);
            Assert.True(LogAttribute.FilePath.EndsWith(@"FilePath\FilePathParameterWithRealTypeTest.cs"));
         };
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

      public class Computer
      {
         public static int Divide(int a, int b)
         {
            var myInt = new MyInt(a);
            return myInt.DivideBy(b);
         }
      }

      public class LogAttribute : Attribute
      {
         public static string FilePath;
         public bool NetAspectAttribute = true;

         public void BeforeCallMethod(string filePath)
         {
             FilePath = filePath;
         }
      }
   }
}
