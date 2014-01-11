using System.Collections.Generic;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Helpers.IL;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model
{
    public class MethodToWeave
    {
        private readonly List<MethodWeavingConfiguration> interceptors;
        private readonly Method method;

        public MethodToWeave(List<MethodWeavingConfiguration> interceptors, Method method)
        {
            this.interceptors = interceptors;
            this.method = method;
        }

        public Method Method
        {
            get { return method; }
        }

        public List<MethodWeavingConfiguration> Interceptors
        {
            get { return interceptors; }
        }
    }
}