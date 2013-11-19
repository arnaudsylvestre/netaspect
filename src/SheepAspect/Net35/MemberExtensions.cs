using System.Collections.Generic;
using System.Reflection;

namespace System
{
   public static class MemberExtensions
   {
       public static IList<CustomAttributeData> GetCustomAttributesData(this MemberInfo memberInfo_P)
       {
          return CustomAttributeData.GetCustomAttributes(memberInfo_P);
       }
   }
}