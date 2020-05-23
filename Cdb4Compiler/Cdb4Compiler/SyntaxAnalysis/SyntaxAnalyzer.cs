using Cdb4Compiler.LexicalAnalysis.Tokens;
using Cdb4Compiler.SyntaxAnalysis.AnalyzerImpl;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.SyntaxAnalysis
{
    public class SyntaxAnalyzer
    {
        public SyntaxAnalyzer(IReadOnlyList<Token> tokens)
        {
            var analyzer = new AscendingLeftmostAnalyzerImpl();
            analyzer.Analyze(tokens);
        }

        // TODO: GetParseTree()

        // TODO: GetAST()

        // TODO: GetErrors()

    }
}
