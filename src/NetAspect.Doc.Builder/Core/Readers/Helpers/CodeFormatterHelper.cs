using System;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;

namespace NetAspect.Doc.Builder.Core.Readers.Helpers
{
    public static class CodeFormatterHelper
    {
        public static string Tabs = "    ";

        public static string ToNetAspectString(this AstNode astNode)
        {
            var cSharpFormattingOptions = FormattingOptionsFactory.CreateAllman();
            var code = astNode.ToString(cSharpFormattingOptions);
            return string.Join(Environment.NewLine, code.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Replace("\t", Tabs)));
        }
        public static string Indent(this string s)
        {
            return string.Join(Environment.NewLine, s.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Select(str => Tabs + str));
        }
    }
}