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

        public void Before<T>(T instance)
        {
            Before2(instance);
            
        }
        public void Before2(object instance)
        {
            instance = "intercepted";
        }
    }
}
