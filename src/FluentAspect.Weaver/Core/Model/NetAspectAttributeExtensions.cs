using System;
using System.Reflection;
using FluentAspect.Weaver.Helpers;

namespace FluentAspect.Weaver.Core.Model
{
    public static class NetAspectAttributeExtensions
    {
       public static NetAspectAttributeKind GetAspectKind(this object o)
       {
          try
          {
             var value = o.GetValueForField("NetAspectAttributeKind");
             return EnumParser.Parse<NetAspectAttributeKind>(value.ToString());
          }
          catch (Exception)
          {
             return NetAspectAttributeKind.Invalid;
          }
       }

    }
}