using System.IO;
using ICSharpCode.NRefactory.CSharp;
using NetAspect.Doc.Builder.Core.GettingStarted;
using NetAspect.Doc.Builder.Model;

namespace NetAspect.Doc.Builder.Core.Readers
{
    public interface ICsFileReaderVisitor<T> : IAstVisitor
    {
        void SetModel(T model);
    }

    public class CsFileReader
    {
        static CSharpParser parser = new CSharpParser();

        public static void ReadCsFile<TVisitor, TModel>(TModel model, string csFilePath)
            where TVisitor : ICsFileReaderVisitor<TModel>, new()
        {
            using (var stream = File.OpenRead(csFilePath))
            {
                
                var syntaxTree = parser.Parse(stream);
                var visitor = new TVisitor();
                visitor.SetModel(model);
                syntaxTree.AcceptVisitor(visitor);
            }
        }


        public static TModel ReadCsFile<TVisitor, TModel>(string csFilePath)
            where TVisitor : ICsFileReaderVisitor<TModel>, new()
            where TModel : new()
        {
            var model = new TModel();
            ReadCsFile<TVisitor, TModel>(model, csFilePath);
            return model;
        }
    }
}