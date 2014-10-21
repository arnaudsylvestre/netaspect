using NetAspect.Doc.Builder.Helpers;

namespace NetAspect.Doc.Builder.Model
{
    public class PutAspectsSectionModel
    {
        private readonly CsTestFile aspectByAttribute;
        private readonly CsTestFile aspectBySelect;

        public PutAspectsSectionModel(CsTestFile aspectByAttribute, CsTestFile aspectBySelect)
        {
            this.aspectByAttribute = aspectByAttribute;
            this.aspectBySelect = aspectBySelect;
        }

        public string WeaveWithAttributeSampleAspect { get { return aspectByAttribute.AspectCode; } }
        public string WeaveWithAttributeSampleClassToWeave { get { return aspectByAttribute.ClassToWeaveCode; } }
        public string WeaveWithSelectSampleAspect { get { return aspectBySelect.ClassToWeaveCode; } }
        public string WeaveWithSelectSampleClassToWeave { get { return aspectBySelect.ClassToWeaveCode; } }
    }
}