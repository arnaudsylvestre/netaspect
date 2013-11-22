using System.Windows.Forms;

namespace FluentAspect.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
           MyClassToWeave toWeave = new MyClassToWeave();
           var waved = toWeave.MustRaiseExceptionAfterWeave();
           MessageBox.Show(waved);
        }
    }
}
