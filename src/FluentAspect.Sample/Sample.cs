using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentAspect.Sample
{
    class Res
    {
        public int field
        {
            set {}
        }
    }   
    
    public class EventClass
    {
        public delegate void MyEventHandler(object sender, MyEventArgs e);

        public event MyEventHandler MyEvent;

        public void RaisesMyEvent()
        {

           if(MyEvent != null) //required in C# to ensure a handler is attached
              MyEvent(this, new MyEventArgs(/*any info you want handlers to have*/));
        }
    }

    public class MyEventArgs
    {
    }

    class Sample
    {
        public static int Beforeinstance;

        public void Before<T>(ref bool instance)
        {
            Before2(instance);
            
        }
        public void Before2(bool instance)
        {
        }
    }
}
