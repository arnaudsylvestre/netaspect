using NetAspect.Doc.Builder.Helpers;

namespace NetAspect.Doc.Builder.Model
{
    public class OtherAssembliesSectionModel
    {
        private readonly CsTestFile weaveOtherAssembly;

        public OtherAssembliesSectionModel(CsTestFile weaveOtherAssembly)
        {
            this.weaveOtherAssembly = weaveOtherAssembly;
        }

        public string SampleAspect { get { return weaveOtherAssembly.AspectCode; } }
        public string SampleClassToWeave { get { return weaveOtherAssembly.ClassToWeaveCode; } }
        public string SampleTest { get { return weaveOtherAssembly.CallCode; } }
    }
}