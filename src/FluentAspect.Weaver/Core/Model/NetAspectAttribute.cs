using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Helpers;

namespace FluentAspect.Weaver.Core.Model
{
   public class NetAspectAttribute
   {
      private object attribute;
      public const BindingFlags _bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

      public NetAspectAttribute(object attribute_P)
      {
         attribute = attribute_P;
      }

      public bool IsValid
      {
         get
         {
            return NetAspectAttributeKindField != null;
         }
      }

      private FieldInfo NetAspectAttributeKindField
      {
         get { return Type.GetField("NetAspectAttributeKind", _bindingFlags); }
      }

      public NetAspectAttributeKind Kind
      {
         get
         {
            return EnumParser.Parse<NetAspectAttributeKind>(NetAspectAttributeKindField.GetValue(attribute).ToString());
         }
      }

      public MethodWeavingConfiguration MethodWeavingConfiguration
      {
         get
         {
            return new MethodWeavingConfiguration(attribute);
         }
      }

      public CallWeavingConfiguration CallWeavingConfiguration
      {
         get
         {
            return new CallWeavingConfiguration(attribute);
         }
      }

      public MethodInfo SelectorMethod
      {
         get { return attribute.GetType().GetMethod("WeaveMethod", _bindingFlags); }
      }

      public Type Type
      {
         get { return attribute.GetType(); }
      }

      public IEnumerable<Assembly> AssembliesToWeave
      {
         get
         {
            var fieldInfo_L = Type.GetField("AssembliesToWeave", _bindingFlags);
            if (fieldInfo_L == null)
               return new Assembly[0];
            return (IEnumerable<Assembly>)fieldInfo_L.GetValue(attribute);
         }
      }
   }
}