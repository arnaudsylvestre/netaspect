using System;
using NetAspect.Weaver.Tests.unit;

namespace NetAspect.Weaver.Tests.docs
{
   public class MethodWeavingForDocTest :
      NetAspectTest<MethodWeavingForDocTest.MyInt>
   {
      protected override Action CreateEnsure()
      {
         return () => { };
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

      public class LogAttribute : Attribute
      {
         public bool NetAspectAttribute = true;
      }
   }
}
