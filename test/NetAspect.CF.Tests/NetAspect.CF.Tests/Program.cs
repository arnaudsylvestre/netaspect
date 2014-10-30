using System;
using System.IO;
using NUnitLite.Runner;

namespace NetAspect.nsAfterCallConstructorParametersTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var writer = new StringWriter();
            new TextUI(writer).Execute(new string[0]);
            var message = writer.ToString();
            if (message.Contains("Errors and Failures"))
            {
                throw new Exception(message);
            }
        }
    }
}
