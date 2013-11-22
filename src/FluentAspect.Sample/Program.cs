using System.Windows.Forms;

namespace FluentAspect.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
           MyClassToWeave toWeave = new MyClassToWeave();
           var waved = toWeave.CheckWithReturn();
           MessageBox.Show(waved);
        }
    }
}
