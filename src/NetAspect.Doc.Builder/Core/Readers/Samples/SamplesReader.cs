using System.Collections.Generic;
using System.IO;
using NetAspect.Doc.Builder.Core.GettingStarted;
using NetAspect.Doc.Builder.Core.Readers.Core;
using NetAspect.Doc.Builder.Core.Readers.Documentation.Sections.PutAspects;
using NetAspect.Doc.Builder.Model;

namespace NetAspect.Doc.Builder.Core.Readers.Documentation
{
    public class SamplesReader
    {
        public static SamplesPageModel Read(string baseFolder)
        {
            var model = new SamplesPageModel();
            var files = Directory.GetFiles(Path.Combine(baseFolder, @"Samples"), "*.cs", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                model.Samples.Add(CsTestFileReader.Read(file));
            }
            return model;
        }
    }
}