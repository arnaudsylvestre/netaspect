﻿using System;
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

        public event Action MyEvent;

        public void RegisterEvent()
        {
            MyEvent();
        }

        public void RaisesMyEvent()
        {
              MyEvent();
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

    class TryFinallySample
    {
        public void Hello()
        {
            try
            {
                int a = 2;
            }
            catch (Exception)
            {
                int a = 3;
            }
            finally
            {
                int a = 4;
            }
        }
    }
}
