using System.Collections.Generic;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Weavers.Helpers;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model
{
    public class MethodToWeave
    {
        private Method method;
        private List<MethodWeavingConfiguration> interceptors;

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