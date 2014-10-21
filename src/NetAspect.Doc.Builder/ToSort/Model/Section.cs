using System.Collections.Generic;

namespace NetAspect.Doc.Builder.Model
{
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
                    private static int idIncrement;

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
                }
            }
        }
    }
}