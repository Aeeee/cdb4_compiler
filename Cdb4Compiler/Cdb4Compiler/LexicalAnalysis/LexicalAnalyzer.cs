using Cdb4Compiler.LexicalAnalysis.Impl;
using Cdb4Compiler.LexicalAnalysis.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.LexicalAnalysis
{
    public class LexicalAnalyzer
    {
        private List<Token> tokens = new List<Token>();
        private List<LexicalError> errors = new List<LexicalError>();

        public LexicalAnalyzer(string sourceText)
        {
            var impl = new LexicalAnalyzerImpl(tokens, errors);
            impl.Analyze(sourceText);
        }

        public IReadOnlyCollection<Token> GetAllTokens()
        {
            return tokens;
        }

        public IReadOnlyCollection<LexicalError> GetAllErrors()
        {
            return errors;
        }

    }
}
