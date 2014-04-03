using System.Windows.Forms;

namespace NetAspect.Sample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var toWeave = new MyClassToWeave();
            string waved = toWeave.CheckWithReturn();
            MessageBox.Show(waved);
        }
    }
}