using NetAspect.Doc.Builder.Helpers;

namespace NetAspect.Doc.Builder.Model
{
    public class DocumentationPageModel
    {
        public PutAspectsSectionModel PutAspects { get; set; }
        public InterceptorsSectionModel Interceptors { get; set; }
        public LifeCyclesSectionModel LifeCycles { get; set; }
        public OtherAssembliesSectionModel OtherAssemblies { get; set; }

        public string GeneratePutAttributesSection()
        {
            return ConfigureNVelocity.With("section", PutAspects)
                                     .AndGenerateInto(Templates.Templates.PutAttributesSection);
        }
        public string GenerateInterceptorsSection()
        {
            return ConfigureNVelocity.With("section", Interceptors)
                                     .AndGenerateInto(Templates.Templates.InterceptorsSection);
        }
        public string GenerateLifeCyclesSection()
        {
            return ConfigureNVelocity.With("section", LifeCycles)
                                     .AndGenerateInto(Templates.Templates.LifeCyclesSection);
        }
        public string GenerateOtherAssembliesSection()
        {
            return ConfigureNVelocity.With("section", OtherAssemblies)
                                     .AndGenerateInto(Templates.Templates.OtherAssembliesSection);
        }

    }
}