using System;

namespace FluentAspect.Sample
{
    public class EventClass
    {
        public delegate void MyEventHandler(object sender, MyEventArgs e);

        public event Action MyEvent;
        public event Action<int,int> MyEventWithParameter;

        public void RegisterEvent()
        {
            GetEvent()();
        }
        public T CreateDefault<T>()
        {
            return default(T);
        }

        private Action GetEvent()
        {
            return MyEvent;
        }

        public void RaisesMyEvent()
        {
            MyEvent();
        }

        public void RaisesMyEventWithParameters()
        {
            MyEventWithParameter(1, 2);
        }
    }
}