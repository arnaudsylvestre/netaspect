using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;

namespace FluentAspect.Weaver.Tests.Core.Model
{
    public interface INetAspectType
    {
       Type BaseType { get; }
       TypeDefinition TypeDefinition { get; }
       IEnumerable<NetAspectField> Fields { get; }
    }
}