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
                return NVelocityHelper.GenerateContent(
                    Template,
                    new NVelocityHelper.NVelocityEntry
                        {
                            Key = "page",
                            Value = Model,
                        },
                    new NVelocityHelper.NVelocityEntry
                        {
                            Key = "template",
                            Value = new TemplateHelper(),
                        });
            }
        }

        public object Model { get; set; }
    }
}