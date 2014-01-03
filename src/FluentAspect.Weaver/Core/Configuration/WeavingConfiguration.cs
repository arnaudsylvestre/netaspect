using System.Collections.Generic;

namespace FluentAspect.Weaver.Core.Configuration
{
    public class WeavingConfiguration
    {
        public WeavingConfiguration()
        {
            Methods = new List<MethodMatch>();
            Constructors = new List<MethodMatch>();
        }

        public List<MethodMatch> Methods { get; private set; }

        public List<MethodMatch> Constructors { get; private set; }
    }
}