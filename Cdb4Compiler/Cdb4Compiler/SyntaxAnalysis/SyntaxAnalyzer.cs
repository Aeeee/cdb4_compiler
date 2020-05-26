using Cdb4Compiler.LexicalAnalysis.Tokens;
using Cdb4Compiler.SyntaxAnalysis.AnalyzerImpl;
using Cdb4Compiler.SyntaxAnalysis.Ast;
using Cdb4Compiler.SyntaxAnalysis.AstBuilderImpl;
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
        private ProgramNode astRoot = null;

        public SyntaxAnalyzer(IReadOnlyList<Token> tokens)
        {
            var analyzer = new AscendingLeftmostAnalyzerImpl();
            analyzer.Analyze(tokens, errors, ref parseTreeRoot);

            var astBuilder = new ParseTreeAstBuilder();
            astRoot = astBuilder.BuildAST(parseTreeRoot);
        }

        public ParseTreeNode GetParseTreeRoot() => parseTreeRoot;

        public ProgramNode GetAstRoot() => astRoot;

        public IReadOnlyList<SyntaxError> GetErrors() => errors;

    }
}
