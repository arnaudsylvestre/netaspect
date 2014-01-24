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
        public static int Beforeinstance;

        public static void Before(ref int instance)
        {
            instance = 3;
        }
    }
}
