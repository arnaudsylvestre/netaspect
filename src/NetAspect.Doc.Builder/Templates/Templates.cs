using System.IO;
using System.Reflection;
using System.Text;

namespace NetAspect.Doc.Builder.Templates
{
    public class Templates
    {
        private static string Read(string resourceName)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof (Templates), resourceName);
            var manifestResourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        public static string PutAttributesSection
        {
            get { return Read("Documentation.Sections.PutAttributesSection.txt"); }
        }

        public static string InterceptorsSection
        {
            get { return Read("Documentation.Sections.InterceptorsSection.txt"); }
        }

        public static string LifeCyclesSection
        {
            get { return Read("Documentation.Sections.LifeCyclesSection.txt"); }
        }

        public static string OtherAssembliesSection
        {
            get { return Read("Documentation.Sections.OtherAssembliesSection.txt"); }
        }

        public static string PageContainer
        {
            get { return Read("PageContainer.txt"); }
        }

        public static string DocumentationPage
        {
            get { return Read("Documentation.DocumentationPage.txt"); }
        }

        public static string GettingStartedPage
        {
            get { return Read("GettingStarted.GettingStartedPage.txt"); }
        }

        public static string HomePage
        {
            get { return Read("Home.HomePage.txt"); }
        }

        public static string NetAspectPage
        {
            get { return Read("NetAspect.NetAspectPage.txt"); }
        }
    }
}