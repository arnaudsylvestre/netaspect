﻿using System.Collections.Generic;
using System.IO;
using NetAspect.Doc.Builder.Core.Readers.Documentation.Sections.PutAspects;
using NetAspect.Doc.Builder.Model;

namespace NetAspect.Doc.Builder.Core.Readers.Documentation
{
    public class DocumentationReader
    {
        public static DocumentationPageModel Read(string baseFolder)
        {
            Dictionary<string, string> parameterDescriptions = ParameterDescriptionFactory.Create();
            return new DocumentationPageModel
                {
                    Interceptors =
                        DocumentationInterceptorsSectionReader.ExtractInterceptors(Path.Combine(baseFolder,
                                                                                                @"Documentation\Interceptors"), parameterDescriptions),
                    PutAspects =
                        DocumentationPutAspectsSectionReader.ExtractPutAspects(Path.Combine(baseFolder, @"Documentation\Weaving")),
                    AvailableParameters =
                        DocumentationAvailableParametersSectionReader.ExtractAvailableParameters(Path.Combine(baseFolder, @"Documentation\Parameters"), parameterDescriptions),
                };
        }
    }
}