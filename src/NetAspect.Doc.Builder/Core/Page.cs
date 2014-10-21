using NetAspect.Doc.Builder.Helpers;

namespace NetAspect.Doc.Builder.Model
{
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
            get
            {
                return ConfigureNVelocity.With("page", Model)
                                         .AndGenerateInto(Template);
            }
        }

        public object Model { get; set; }
    }
}