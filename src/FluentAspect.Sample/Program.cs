using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FluentAspect.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
           string file = @"C:\Test.txt";
           MyClassToWeave toWeave = new MyClassToWeave();
           try
           {
              toWeave.MustRaiseExceptionAfterWeave();
              MessageBox.Show("Not raised");
              throw new Exception();
           }
           catch (RaiseException)
           {
              MessageBox.Show("OK !");
           }
           catch
           {
              MessageBox.Show("Another exception thrown");
              throw;
           }
        }
    }
}
