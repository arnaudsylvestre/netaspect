using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NetAspect.Doc.Builder.Core.Readers.Documentation.Sections.PutAspects
{
    public static class DirectoryExtensions
    {
        public static IEnumerable<string> GetAllCsFiles(this string baseFolder)
        {
            return Directory.GetFiles(baseFolder, "*.cs", SearchOption.AllDirectories).OrderBy(Path.GetFileName).ToList();
        }
    }
}