using System.Reflection;
using FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Parameters.Before.Instance;

namespace FluentAspect.Weaver.Tests.unit
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