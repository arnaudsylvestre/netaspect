using System.Collections.Generic;
using System.IO;
using ICSharpCode.NRefactory.CSharp;
using NetAspect.Doc.Builder;
using NetAspect.Doc.Builder.Helpers;

namespace NetAspect.SamplesExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new CSharpParser();
            var samples = new List<Sample>();
            var sampleFolders = args[0];
            var destinationFolder = args[1];
            foreach (var file in Directory.GetFiles(sampleFolders, "*.cs", SearchOption.AllDirectories))
            {
                using (var stream = File.OpenRead(file))
                {
                    var syntaxTree = parser.Parse(stream);
                    var sample = new Sample();
                    syntaxTree.AcceptVisitor(new SampleVisitor(sample));
                    samples.Add(sample);
                }
            }

            foreach (var sample in samples)
            {
                var sampleCs = ConfigureNVelocity.With("sample", sample).AndGenerateInto(Templates.SampleTemplates.Sample);
                File.WriteAllText(Path.Combine(destinationFolder, sample.Name + ".cs.pp"), sampleCs);
            }
        }
    }
}
