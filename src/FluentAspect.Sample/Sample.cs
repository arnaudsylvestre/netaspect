using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentAspect.Sample
{
    class Sample
    {
        public static object Beforeinstance;

        public void Before(ref object instance)
        {
            Beforeinstance = instance;
        }
    }
}
