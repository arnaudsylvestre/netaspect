using System.Collections.Generic;
using System.IO;
using NetAspect.Doc.Builder.Core.Readers.Documentation.Sections.PutAspects;
using NetAspect.Doc.Builder.Model;

namespace NetAspect.Doc.Builder.Core.Readers.Documentation
{
    public class DocumentationReader
    {
        public static DocumentationPageModel Read(string baseFolder)
        {
            var parameterDescriptions = ParameterDescriptionFactory.Create();
            //var availableParametersSectionModel = DocumentationAvailableParametersSectionReader.ExtractAvailableParameters(Path.Combine(baseFolder, @"Documentation\Parameters"), parameterDescriptions);
            AvailableParametersSectionModel availableParametersSectionModel = null;
            return new DocumentationPageModel
                {
                    Interceptors =
                        DocumentationInterceptorsSectionReader.ExtractInterceptors(Path.Combine(baseFolder,
                                                                                                @"Documentation\Interceptors"), parameterDescriptions, availableParametersSectionModel),
                    PutAspects =
                        DocumentationPutAspectsSectionReader.ExtractPutAspects(Path.Combine(baseFolder, @"Documentation\Weaving")),
                    LifeCycles =
                        DocumentationLifeCyclesSectionReader.ExtractLifeCycles(Path.Combine(baseFolder, @"Documentation\LifeCycles")),
                };
        }
    }
}