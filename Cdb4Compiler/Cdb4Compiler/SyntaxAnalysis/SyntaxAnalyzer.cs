using Cdb4Compiler.LexicalAnalysis.Tokens;
using Cdb4Compiler.SyntaxAnalysis.AnalyzerImpl;
using Cdb4Compiler.SyntaxAnalysis.ParseTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.SyntaxAnalysis
{
    public class SyntaxAnalyzer
    {
        private List<SyntaxError> errors = new List<SyntaxError>();
        private ParseTreeNode parseTreeRoot = null;

        public SyntaxAnalyzer(IReadOnlyList<Token> tokens)
        {
            var analyzer = new AscendingLeftmostAnalyzerImpl();
            analyzer.Analyze(tokens, errors, ref parseTreeRoot);
        }

        public ParseTreeNode GetParseTreeRoot() => parseTreeRoot;

        // TODO: GetAST()

        public IReadOnlyList<SyntaxError> GetErrors() => errors;

    }
}
