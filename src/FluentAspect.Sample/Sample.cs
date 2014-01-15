using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentAspect.Sample
{
    class Res
    {
        public int field;
    }        

    class Sample
    {
        public static object Beforeinstance;

        public static void Before(ref object instance)
        {
            Beforeinstance = instance;
        }
    }
}
