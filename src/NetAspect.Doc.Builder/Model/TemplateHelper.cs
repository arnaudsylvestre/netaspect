using NetAspect.Doc.Builder.Helpers;

namespace NetAspect.Doc.Builder.Model
{
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
            return NVelocityHelper.GenerateContent(
                paragraph.DescriptionTemplate,
                new NVelocityHelper.NVelocityEntry
                    {
                        Key = "paragraph",
                        Value = paragraph.Model,
                    },
                new NVelocityHelper.NVelocityEntry
                    {
                        Key = "template",
                        Value = this,
                    });
        }

        public string GenerateDescription(Section section)
        {
            return NVelocityHelper.GenerateContent(
                section.DescriptionTemplate,
                new NVelocityHelper.NVelocityEntry
                    {
                        Key = "paragraph",
                        Value = section.Model,
                    },
                new NVelocityHelper.NVelocityEntry
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
}