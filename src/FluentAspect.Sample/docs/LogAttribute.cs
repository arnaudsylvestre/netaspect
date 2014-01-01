using System;

namespace FluentAspect.Sample.docs
{
    public class LogAttribute : Attribute
    {
        public bool IsNetAspectAttribute { get { return true; } }
    }
}