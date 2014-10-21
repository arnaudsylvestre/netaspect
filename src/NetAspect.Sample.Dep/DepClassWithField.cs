using System;
using System.Reflection;

namespace NetAspect.Sample.Dep
{
    public class TotoAttribute : Attribute
    {
        public const uint T = 125;

        public TotoAttribute(uint t)
        {
            
        }
    }

   public class DepClassWithField
   {
      public string Field;
       [TotoAttribute(TotoAttribute.T)]
      public void Test()
      {
         ParameterInfo toto = MethodBase.GetCurrentMethod().GetParameters()[3];
          var toto2 = typeof (DepClassWithProperty);
          new TotoAttribute((uint)0xfffffffe);

      }
   }

   public class DepClassWithProperty
   {
      public string Property { get; set; }
   }
}
