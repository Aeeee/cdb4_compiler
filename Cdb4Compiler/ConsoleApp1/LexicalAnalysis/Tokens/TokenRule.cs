using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.LexicalAnalysis.Tokens
{
    class TokenRule
    {
        public TokenType Type { get; private set; }
        public string Pattern { get; private set; }

        public TokenRule(TokenType type, string pattern)
        {
            Type = type;
            Pattern = pattern;
        }
    }
}

