using System.Collections.Generic;
using NetAspect.Doc.Builder.Helpers;

namespace NetAspect.Doc.Builder.Model
{
    public class SamplesPageModel
    {
        public SamplesPageModel()
        {
            Samples = new List<CsTestFile>();
        }

        public List<CsTestFile> Samples { get; set; }
        

    }
}