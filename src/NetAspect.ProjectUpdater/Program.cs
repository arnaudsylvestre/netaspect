using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace NetAspect.ProjectUpdater
{
   static class Program
   {
      /// <summary>
      /// Point d'entrée principal de l'application.
      /// </summary>
      [STAThread]
      static void Main(string[] args)
      {
         List<string> projectFilesToWeave = new List<string>();
         if (args.Length < 1)
         OpenFileDialog openFileDialog = new OpenFileDialog();
         if (openFileDialog.ShowDialog() == DialogResult.OK)
         {
            
         }
      }
   }
}
