using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Helpers;

namespace FluentAspect.Weaver.Core.Model
{
    public class NetAspectAttribute
    {
        public const BindingFlags BINDING_FLAGS =
            BindingFlags.NonPublic | BindingFlags.Public|
            BindingFlags.Instance | BindingFlags.Static;

        private readonly object attribute;

        public NetAspectAttribute(object attribute_P)
        {
            attribute = attribute_P;
        }

        public bool IsValid
        {
            get { return NetAspectAttributeKindField != null; }
        }

        public NetAspectAttributeKind Kind
        {
            get
            {
                return
                    EnumParser.Parse<NetAspectAttributeKind>(NetAspectAttributeKindField.GetValue(attribute).ToString());
            }
        }

        private FieldInfo NetAspectAttributeKindField
        {
           get { return attribute.GetType().GetField("NetAspectAttributeKind", BINDING_FLAGS); }
        }

        public MethodWeavingConfiguration MethodWeavingConfiguration
        {
            get { return new MethodWeavingConfiguration(attribute); }
        }

        public CallWeavingConfiguration CallWeavingConfiguration
        {
            get { return new CallWeavingConfiguration(attribute); }
        }
    }
}