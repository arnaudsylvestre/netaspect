using System;

namespace FluentAspect.Sample
{
    internal class Res
    {
        public int field
        {
            set { }
        }
    }

    public class EventClass
    {
        public delegate void MyEventHandler(object sender, MyEventArgs e);

        public event Action MyEvent;
        public event Action<int,int> MyEventWithParameter;

        public void RegisterEvent()
        {
            GetEvent()();
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

    public class MyEventArgs
    {
    }

    internal class Sample
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

    internal class TryFinallySample
    {
        public string Hello()
        {
            try
            {
                return "Hello";
            }
            catch (Exception e)
            {
                string a = e.Message;
                throw;
            }
            finally
            {
                int b = 2;
            }
        }
    }
}