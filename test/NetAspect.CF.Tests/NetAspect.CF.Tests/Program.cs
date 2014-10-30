using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using NUnitLite.Runner;

namespace NetAspect.CF.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var writer = new StringWriter();
            new TextUI(writer).Execute(new string[0]);
            if (writer.ToString().Contains("Errors and Failures"))
            {
                throw new Exception(writer.ToString());
            }
        }
    }
}
