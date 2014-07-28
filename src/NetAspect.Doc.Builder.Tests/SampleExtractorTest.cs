using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NetAspect.Doc.Builder.Tests.Resources;

namespace NetAspect.Doc.Builder.Tests
{
    [TestFixture]
    public class SampleExtractorTest
    {
        [Test]
        public void CheckSimpleExtractor()
        {
            var sampleExtractor = new SampleExtractor();
            sampleExtractor.ExtractSample(new MemoryStream(Samples.Sample1));


        }
    }
}
