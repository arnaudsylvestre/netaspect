using System.Windows.Forms;

namespace FluentAspect.Sample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            A.MyClassToWeave t = new A.MyClassToWeave();
            t.MyMethodToWeave();

            var toWeave = new MyClassToWeave();
            string waved = toWeave.CheckWithReturn();
            MessageBox.Show(waved);
        }
    }
}