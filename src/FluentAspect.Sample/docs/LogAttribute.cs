﻿using System;
using System.Text;

namespace FluentAspect.Sample.docs
{
    public class LogAttribute : Attribute
    {
        public static StringBuilder Trace = new StringBuilder();

        private string NetAspectAttributeKind = "MethodWeaving";

        public void Before(MyInt instance, int v)
        {
            Trace.Append(string.Format("Start Division {0} / {1}", instance.Value, v));
        }

        public void OnException(Exception exception)
        {
            Console.WriteLine("Exception : {0}", exception.Message);
        }
    }
}