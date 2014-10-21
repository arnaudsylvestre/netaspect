using System.IO;
using NetAspect.Doc.Builder.Helpers;
using NetAspect.Doc.Builder.Model;

namespace NetAspect.Doc.Builder.Core
{
    public class WebsiteGenerator
    {
        public void Generate(WebSite website, string folder)
        {
            foreach (var page in website.Pages)
            {
                string filePath = Path.Combine(folder, string.Format("{0}.html", page.Name));
                File.WriteAllText(
                    filePath,
                    ConfigureNVelocity.With("page", page)
                                      .AndWith("website", website)
                                      .AndGenerateInto(Templates.Templates.PageContainer)
                    );
            }
        }
    }
}