using NetAspect.Doc.Builder.Helpers;

namespace NetAspect.Doc.Builder.Model
{
    public class DocumentationPageModel
    {
        public PutAspectsSectionModel PutAspects { get; set; }
        public InterceptorsSectionModel Interceptors { get; set; }
        public AvailableParametersSectionModel AvailableParameters { get; set; }

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
        public string GenerateAvailableParametersSection()
        {
            return ConfigureNVelocity.With("section", AvailableParameters)
                                     .AndGenerateInto(Templates.Templates.AvailableParametersSection);
        }
    }
}