using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using NetAspect.Doc.Builder.Core.GettingStarted;
using NetAspect.Doc.Builder.Helpers;
using NetAspect.Doc.Builder.Resources;
using NetAspect.Doc.Builder.Templates.Documentation;

namespace NetAspect.Doc.Builder.Model
{
    public static class WebsiteFactory
    {
        public static WebSite Create(string baseFolder)
        {
            var webSite = new WebSite();
            webSite.Pages.Add(CreateHomePage());
            webSite.Pages.Add(CreateNetAspectPage());
            webSite.Pages.Add(CreateGettingStartedPage(baseFolder));
            webSite.Pages.Add(CreateDocumentationPage(baseFolder));
            return webSite;
        }

        private static Page CreateDocumentationPage(string baseFolder)
        {
            var extractor = new DocumentationFromTestExtractor();
            return new Page(
                "Documentation",
                Templates.Templates.DocumentationPage,
                "Documentation",
                new DocumentationPageModel
                    {
                        Interceptors = extractor.ExtractInterceptors(Path.Combine(baseFolder, @"Documentation\Interceptors")),
                        Weaving = extractor.ExtractWeaving(Path.Combine(baseFolder, @"Documentation\Weaving")),
                        Parameters = extractor.ExtractParameters(Path.Combine(baseFolder, @"Documentation\Parameters")),
                    });
        }

        private static List<Section> Convert(DocumentationFromTest tests)
        {
            var availablesSections = new List<Section>();
            availablesSections.Add(CreateInterceptorsSection(tests));
            return availablesSections;
        }

        private static Section CreateInterceptorsSection(DocumentationFromTest tests)
        {
            var serializer = new XmlSerializer(typeof (DocumentationConfiguration));
            var docPage = (DocumentationConfiguration) serializer.Deserialize(new StringReader(Content.DocumentationPage));
            var paragraphs = new List<Section.Paragraph>();

            foreach (InterceptorKind interceptorKindConfiguration in docPage.InterceptorKinds)
            {
                paragraphs.Add(CreateMethodInterceptorParagraph(tests, interceptorKindConfiguration));
            }

            return new Section(paragraphs, "Interceptors", null, DocumentationTemplates.SectionDescriptionInterceptors);
        }

        private static Section.Paragraph CreateMethodInterceptorParagraph(DocumentationFromTest tests, InterceptorKind interceptorKindConfiguration)
        {
            var subParagraphs = new List<Section.Paragraph.SubParagraph>();
            foreach (InterceptorKindConfiguration interceptorConfiguration in interceptorKindConfiguration.Configurations)
            {
                //interceptorConfiguration.
                var details = new List<Section.Paragraph.SubParagraph.Detail>();
                //subParagraphs.Add(new Section.Paragraph.SubParagraph(details, interceptorConfiguration.Title, DocumentationTemplates.SubParagraphDescriptionMethodInterceptor, new SubParagraphModel
                //    {
                //        Member = interceptorConfiguration.Member
                //    }));
            }
            return new Section.Paragraph(subParagraphs, interceptorKindConfiguration.Name, null, DocumentationTemplates.ParagraphDescriptionMethodInterceptor);
        }

        private static Page CreateGettingStartedPage(string baseFolder)
        {
            var reader = new GettingStartedReader();
            var model = new GettingStartedPageModel();
            reader.Read(model, Path.Combine(baseFolder, @"GettingStarted\GettingStartedPart1Test.cs"), Path.Combine(baseFolder, @"GettingStarted\GettingStartedPart2Test.cs"));
            return new Page("Getting Started", Templates.Templates.GettingStartedPage, "GettingStarted", model);
        }

        private static Page CreateNetAspectPage()
        {
            return new Page("NetAspect", Templates.Templates.NetAspectPage, "NetAspect", null);
        }

        private static Page CreateHomePage()
        {
            return new Page("Home", Templates.Templates.HomePage, "index", null);
        }

        public class DocumentationPageModel
        {
            private readonly Dictionary<string, string> parameterDescriptions = new Dictionary<string, string>();
            public List<InterceptorDocumentation> Interceptors { get; set; }

            public IEnumerable<string> Members
            {
                get { return Interceptors.Select(i => i.Member).Distinct(); }
            }

            public List<ParameterModel> Parameters { get; set; }
            public WeavingModelDoc Weaving { get; set; }

            public IEnumerable<InterceptorDocumentation> GetInterceptors(string member)
            {
                return from i in Interceptors where i.Member == member select i;
            }

            public void SetParameterDescription(string parameterName, string description)
            {
                parameterDescriptions.Add(parameterName, description);
            }

            public string GetParameterDescription(string parameterName)
            {
                return parameterDescriptions[parameterName];
            }
        }
    }
}