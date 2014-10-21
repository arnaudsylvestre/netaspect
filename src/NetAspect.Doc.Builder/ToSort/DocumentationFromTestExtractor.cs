using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;
using NetAspect.Doc.Builder.Helpers;
using NetAspect.Doc.Builder.Model;

namespace NetAspect.Doc.Builder
{
    public class DocumentationFromTestExtractor
    {
        public WeavingModelDoc ExtractWeaving(string directoryPath_P)
        {
            var doc = new WeavingModelDoc();
            var parser = new CSharpParser();
            using (FileStream stream = File.OpenRead(Path.Combine(directoryPath_P, "WeaveWithAttributeSampleTest.cs")))
            {
                SyntaxTree syntaxTree = parser.Parse(stream);
                var test = new InterceptorDocumentation();
                syntaxTree.AcceptVisitor(new InterceptorDocumentationVisitor(test));
                doc.WeaveWithAttributeSampleAspect = test.AspectCode;
                doc.WeaveWithAttributeSampleClassToWeave = test.ClassToWeaveCode;
            }
            using (FileStream stream = File.OpenRead(Path.Combine(directoryPath_P, "WeaveWithSelectSampleTest.cs")))
            {
                SyntaxTree syntaxTree = parser.Parse(stream);
                var test = new InterceptorDocumentation();
                syntaxTree.AcceptVisitor(new InterceptorDocumentationVisitor(test));
                if (test.Name != null)
                    doc.WeaveWithSelectSampleAspect = test.AspectCode;
                doc.WeaveWithSelectSampleClassToWeave = test.ClassToWeaveCode;
            }

            return doc;
        }

        public List<InterceptorDocumentation> ExtractInterceptors(string directoryPath_P)
        {
            var doc = new List<InterceptorDocumentation>();
            var parser = new CSharpParser();
            IOrderedEnumerable<string> files_L = Directory.GetFiles(directoryPath_P, "*.cs", SearchOption.AllDirectories).OrderBy(Path.GetFileName);
            foreach (string file_L in files_L)
            {
                using (FileStream stream = File.OpenRead(file_L))
                {
                    SyntaxTree syntaxTree = parser.Parse(stream);
                    var test = new InterceptorDocumentation();
                    syntaxTree.AcceptVisitor(new InterceptorDocumentationVisitor(test));
                    if (test.Name != null)
                        doc.Add(test);
                }
            }


            return doc;
        }

        public List<ParameterModel> ExtractParameters(string directoryPath_P)
        {
            var doc = new List<ParameterModel>();
            var parser = new CSharpParser();
            IOrderedEnumerable<string> files_L = Directory.GetFiles(directoryPath_P, "*.cs", SearchOption.AllDirectories).OrderBy(Path.GetFileName);
            foreach (string file_L in files_L)
            {
                using (FileStream stream = File.OpenRead(file_L))
                {
                    SyntaxTree syntaxTree = parser.Parse(stream);
                    var test = new InterceptorDocumentation();
                    syntaxTree.AcceptVisitor(new InterceptorDocumentationVisitor(test));
                    ParameterModel model = doc.FirstOrDefault(p => p.Name == test.Parameters[0]);
                    if (model == null)
                    {
                        model = new ParameterModel
                            {
                                Name = test.Parameters[0],
                                Samples = new List<ParameterModel.ParameterSample>()
                            };
                        doc.Add(model);
                    }
                    model.Samples.Add(
                        new ParameterModel.ParameterSample
                            {
                                Description = test.When,
                                AspectCode = test.AspectCode,
                                CallCode = test.CallCode,
                                ClassToWeaveCode = test.ClassToWeaveCode,
                            });
                }
            }


            return doc;
        }
    }
}