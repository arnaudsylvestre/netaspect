using System;
using Mono.Cecil;

namespace FluentAspect.Weaver.Tests.Core.Model
{
    public class NetAspectAspect
    {
        private readonly NetAspectClass _netAspectClass;

        public NetAspectAspect(NetAspectClass netAspectClass)
        {
            _netAspectClass = netAspectClass;
        }

        public void AddDefaultConstructor()
        {
            _netAspectClass.AddConstructor();
        }

        public MethodReference DefaultConstructor
        {
            get { return _netAspectClass.DefaultConstructor; }
        }
    }
}