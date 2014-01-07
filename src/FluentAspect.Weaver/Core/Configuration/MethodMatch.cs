using System;
using System.Collections.Generic;
using FluentAspect.Weaver.Core.Model;

namespace FluentAspect.Weaver.Core.Configuration
{
    public class MethodMatch
    {
        public Func<IMethod, bool> Matcher { get; set; }

        public List<MethodWeavingConfiguration> MethodWeavingInterceptors { get; set; }
        public List<CallWeavingConfiguration> CallWeavingInterceptors { get; set; }
    }
}