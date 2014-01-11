using System.Reflection;
using FluentAspect.Weaver.Tests.Core;

namespace FluentAspect.Weaver.Tests.acceptance
{
    public class DoAcceptanceHelper
    {
        private Assembly _assembly;


        public DoAcceptanceHelper(Assembly assembly)
        {
            _assembly = assembly;
        }

        public NetAspectAttribute GetNetAspectAttribute(string name)
        {
            return new NetAspectAttribute(_assembly, name);
        }
    }
}