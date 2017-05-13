using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using SyntaxTree.SyntaxNodes;

namespace GPPGParser
{
    public static class ParserWrap
    {
        public static SyntaxNode Parse(string text)
        {
            Scanner scanner = new Scanner();
            scanner.SetSource(text, 0);

            Parser parser = new Parser(scanner);

            var succeeded = parser.Parse();

            return succeeded ? parser.Root : null;
        }
    }
}
