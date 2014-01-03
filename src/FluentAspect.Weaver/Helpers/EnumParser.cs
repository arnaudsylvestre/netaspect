using System;

namespace FluentAspect.Weaver.Helpers
{
    public class EnumParser
    {
        public static T Parse<T>(string value)
        {
            return (T) Enum.Parse(typeof (T), value, true);
        }
    }
}