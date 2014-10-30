using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NetAspect.Doc.Builder.Core.Readers.Core;
using NetAspect.Doc.Builder.Helpers;

namespace NetAspect.TestsExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = Directory.GetFiles(args[0], "*.cs", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var testFile = CsTestFileReader.Read(file);
                if (testFile.CanBeRun && testFile.Name != null)
                {
                    var content = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof (Program), "TestTemplate.txt")).ReadToEnd();
                    var toWrite = ConfigureNVelocity.With("namespace", "NetAspect")
                                      .AndWith("test", testFile)
                                      .AndGenerateInto(content);
                    var outputPath = Path.Combine(args[1], testFile.TestName + ".cs");
                    File.WriteAllText(outputPath, toWrite);
                }
            }
        }
    }
}
