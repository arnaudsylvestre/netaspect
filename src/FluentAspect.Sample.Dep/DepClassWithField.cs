using System.Reflection;

namespace NetAspect.Sample.Dep
{
   public class DepClassWithField
   {
      public string Field;

      public void Test()
      {
         ParameterInfo toto = MethodBase.GetCurrentMethod().GetParameters()[3];
      }
   }

   public class DepClassWithProperty
   {
      public string Property { get; set; }
   }
}
