using System.Collections.Generic;
using NetAspect.Doc.Builder.Helpers;

namespace NetAspect.Doc.Builder.Model
{
    public class WebSite
    {
        public List<Page> Pages { get; set; } 
    }

    public class Page
    {
        public Page(string title, string template, string name)
        {
            Template = template;
            Title = title;
            Name = name;
        }

        private string Template { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }

        public string Content
        {
            get { return NVelocityHelper.GenerateContent("page", this, Template); }
            
        }

    }

    public class DocumentationPage : Page
    {
        public DocumentationPage():base("Documentation", Templates.Templates.DocumentationPage, "Documentation")
        {
        }


        public class Section
        {
            public string Name { get; set; }
            public string Description { get; set; }

            public class Paragraph
            {
                public string Title { get; set; }

                public string Description { get; set; }

                public class SubParagraph
                {
                    public string Title { get; set; }
                    public string Description { get; set; }

                    public class Detail
                    {
                        public string Title { get; set; }

                        public string Template { get; set; }
                        public object Model { get; set; }
                    }
                }
            }
        }
    }

    public class GettingStartedPage : Page
    {

        public GettingStartedPage()
            : base("Getting Started", Templates.Templates.GettingStartedPage, "GettingStarted")
        {
        }

    }

    public class HomePage : Page
    {

        public HomePage()
            : base("NetAspect", Templates.Templates.HomePage, "index")
        {
        }

    }


}