using NetAspect.Doc.Builder.Helpers;

namespace NetAspect.Doc.Builder.Model
{
    public class LifeCyclesSectionModel
    {
        private readonly CsTestFile transient;
        private readonly CsTestFile perInstance;
        private readonly CsTestFile perClass;

        public LifeCyclesSectionModel(CsTestFile perClass, CsTestFile perInstance, CsTestFile transient)
        {
            this.perClass = perClass;
            this.perInstance = perInstance;
            this.transient = transient;
        }

        public string TransientSampleAspect { get { return transient.AspectCode; } }
        public string TransientSampleClassToWeave { get { return transient.ClassToWeaveCode; } }
        public string TransientSampleTest { get { return transient.CallCode; } }
        public string PerInstanceSampleAspect { get { return perInstance.ClassToWeaveCode; } }
        public string PerInstanceSampleClassToWeave { get { return perInstance.ClassToWeaveCode; } }
        public string PerInstanceSampleTest { get { return perInstance.CallCode; } }
        public string PerTypeSampleAspect { get { return perClass.ClassToWeaveCode; } }
        public string PerTypeSampleClassToWeave { get { return perClass.ClassToWeaveCode; } }
        public string PerTypeSampleTest { get { return perClass.CallCode; } }
    }
}