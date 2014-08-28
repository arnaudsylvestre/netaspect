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

    public class SubParagraphModel
    {
        public string Member { get; set; }
    }

    public class TemplateHelper
    {
        public string GenerateDescription(Section.Paragraph.SubParagraph.Detail detail)
        {
            return NVelocityHelper.GenerateContent(detail.DescriptionTemplate, "detail", detail.Model);
        }
        public string GenerateDescription(Section.Paragraph.SubParagraph subParagraph)
        {
            return NVelocityHelper.GenerateContent(subParagraph.DescriptionTemplate, "subParagraph", subParagraph.Model);
        }
        public string GenerateDescription(Section.Paragraph paragraph)
        {
            return NVelocityHelper.GenerateContent(paragraph.DescriptionTemplate, new NVelocityHelper.NVelocityEntry()
            {
                Key = "paragraph",
                Value = paragraph.Model,
            }, new NVelocityHelper.NVelocityEntry()
            {
                Key = "template",
                Value = this,
            });
        }
        public string GenerateDescription(Section section)
        {
            return NVelocityHelper.GenerateContent(section.DescriptionTemplate, new NVelocityHelper.NVelocityEntry()
            {
                Key = "paragraph",
                Value = section.Model,
            }, new NVelocityHelper.NVelocityEntry()
            {
                Key = "template",
                Value = this,
            });
        }
        public string GenerateCode(Section.Paragraph.SubParagraph.Detail.Sample sample)
        {
            return NVelocityHelper.GenerateContent(sample.Template, "sample", sample.Model);
        }

    }

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
            return new Page("Documentation", Templates.Templates.DocumentationPage, "Documentation", new DocumentationPageModel
            {
               Interceptors = extractor.ExtractInterceptors(Path.Combine(baseFolder, @"Documentation\Interceptors")),
               Weaving = extractor.ExtractWeaving(Path.Combine(baseFolder, @"Documentation\Weaving")),
               Parameters = extractor.ExtractParameters(Path.Combine(baseFolder, @"Documentation\Parameters")),
                });
        }

        public class DocumentationPageModel
        {
            public List<InterceptorDocumentation> Interceptors { get; set; }

           public IEnumerable<string> Members
           {
              get { return Interceptors.Select(i => i.Member).Distinct(); }
           }

           public IEnumerable<InterceptorDocumentation> GetInterceptors(string member)
           {
              return from i in Interceptors where i.Member == member select i;
           }

           public List<ParameterModel> Parameters { get; set; }
           public WeavingModel Weaving { get; set; }

            Dictionary<string, string> parameterDescriptions = new Dictionary<string, string>(); 

            public void SetParameterDescription(string parameterName, string description)
            {
                parameterDescriptions.Add(parameterName, description);
            }

            public string GetParameterDescription(string parameterName)
            {
                return parameterDescriptions[parameterName];
            }
        }

        private static List<Section> Convert(DocumentationFromTest tests)
        {
            var availablesSections = new List<Section>();
            availablesSections.Add(CreateInterceptorsSection(tests));
            return availablesSections;
        }

        private static Section CreateInterceptorsSection(DocumentationFromTest tests)
        {
            var serializer = new XmlSerializer(typeof(DocumentationConfiguration));
            var docPage = (DocumentationConfiguration)serializer.Deserialize(new StringReader(Content.DocumentationPage));
            var paragraphs = new List<Section.Paragraph>();

            foreach (var interceptorKindConfiguration in docPage.InterceptorKinds)
            {
                paragraphs.Add(CreateMethodInterceptorParagraph(tests, interceptorKindConfiguration));
                
            }

            return new Section(paragraphs, "Interceptors", null, DocumentationTemplates.SectionDescriptionInterceptors);
        }

        private static Section.Paragraph CreateMethodInterceptorParagraph(DocumentationFromTest tests, InterceptorKind interceptorKindConfiguration)
        {
            var subParagraphs = new List<Section.Paragraph.SubParagraph>();
            foreach (var interceptorConfiguration in interceptorKindConfiguration.Configurations)
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
    }

   public class ParameterModel
   {
      public class ParameterSample
      {
         public string ClassToWeaveCode { get; set; }
         public string AspectCode { get; set; }
         public string CallCode { get; set; }

          public string Description { get; set; }
      }

      public string Name { get; set; }
      public List<ParameterSample> Samples { get; set; }
   }

   public class WeavingModel
   {
      public string WeaveWithAttributeSampleAspect { get; set; }
      public string WeaveWithAttributeSampleClassToWeave { get; set; }
      public string WeaveWithSelectSampleAspect { get; set; }
      public string WeaveWithSelectSampleClassToWeave { get; set; }
   }


   public class WebSite
    {
        public WebSite()
        {
            Pages = new List<Page>();
        }

        public List<Page> Pages { get; set; } 
    }

    public class Page
    {
        public Page(string title, string template, string name, object model)
        {
            Model = model;
            Template = template;
            Title = title;
            Name = name;
        }

        private string Template { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }

        public string Content
        {
            get { return NVelocityHelper.GenerateContent(Template, new NVelocityHelper.NVelocityEntry()
                {
                    Key = "page",
                    Value = Model,
                }, new NVelocityHelper.NVelocityEntry()
                {
                    Key = "template",
                    Value = new TemplateHelper(),
                });
            }
            
        }

        public object Model { get; set; }
    }



        public class Section
        {
            public Section(List<Paragraph> paragraphs, string name, object model, string descriptionTemplate)
            {
                DescriptionTemplate = descriptionTemplate;
                Model = model;
                Paragraphs = paragraphs;
                Name = name;
            }

            public string Name { get; set; }

            public List<Paragraph> Paragraphs { get; set; }

            public object Model { get; set; }

            public string DescriptionTemplate { get; set; }

            public class Paragraph
            {
                private static int i;

                public Paragraph(List<SubParagraph> subParagraphs, string title, object model, string descriptionTemplate)
                {
                    DescriptionTemplate = descriptionTemplate;
                    Model = model;
                    SubParagraphs = subParagraphs;
                    Title = title;
                    Id = "Paragraph" + i++;
                }

                public List<SubParagraph> SubParagraphs { get; set; }

                public string Id { get; set; }
                public string Title { get; set; }

                public object Model { get; set; }

                public string DescriptionTemplate { get; set; }


                public class SubParagraph
                {
                    public SubParagraph(List<Detail> details, string title, string descriptionTemplate, object model)
                    {
                        Model = model;
                        DescriptionTemplate = descriptionTemplate;
                        Details = details;
                        Title = title;
                    }

                    public string Title { get; private set; }
                    
                    public List<Detail> Details { get; private set; }

                    public string DescriptionTemplate { get; private set; }

                    public object Model { get; private set; }

                    public class Detail
                    {
                        public class Sample
                        {
                            public Sample(string description, string template, object model)
                            {
                                Description = description;
                                Template = template;
                                Model = model;
                            }

                            public string Description { get; set; }

                            public string Template { get; set; }

                            public object Model { get; set; }
                        }

                        private static int idIncrement = 0;

                        public Detail(List<Sample> samples, string title, object model, string descriptionTemplate)
                        {
                            Samples = samples;
                            Title = title;
                            Model = model;
                            DescriptionTemplate = descriptionTemplate;
                            Id = "Detail" + idIncrement++;
                        }

                        public List<Sample> Samples { get; set; }

                        public string Title { get; set; }
                        public string Id { get; private set; }
                        public string DescriptionTemplate { get; set; }

                        public object Model { get; set; }
                    }
                }
            }
        }
    



    public class GettingStartedPageModel
    {
        public string CodeWithoutAspect { get; set; }
        public string AspectCode { get; set; }
        public string CodeWithAspect { get; set; }
        public string TestWithAspect { get; set; }
    }


}