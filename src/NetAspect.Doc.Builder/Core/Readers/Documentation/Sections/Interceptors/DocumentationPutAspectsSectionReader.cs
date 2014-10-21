using System.Linq;
using NetAspect.Doc.Builder.Core.Readers.Core;
using NetAspect.Doc.Builder.Helpers;
using NetAspect.Doc.Builder.Model;

namespace NetAspect.Doc.Builder.Core.Readers.Documentation.Sections.PutAspects
{
    public static class DocumentationInterceptorsSectionReader
    {
        public static InterceptorsSectionModel ExtractInterceptors(string directoryPath)
        {
            return new InterceptorsSectionModel(directoryPath.GetAllCsFiles().
                                                              Select(file_L => CsTestFileReader.Read(file_L)).
                                                              // TODO : Voir si on peut supprimer le test.Name != null
                                                              Where(test => test.Name != null)
                                                              .ToList());
        } 
    }
}