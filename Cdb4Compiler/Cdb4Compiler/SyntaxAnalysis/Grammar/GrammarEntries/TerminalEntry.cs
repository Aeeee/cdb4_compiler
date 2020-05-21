using Cdb4Compiler.LexicalAnalysis.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.SyntaxAnalysis.Grammar.GrammarEntries
{
    public class TerminalEntry : GrammarEntry
    {
        public TokenType Type { get; private set; }
        public string RequiredValue { get; private set; }

        public TerminalEntry(TokenType type, string requiredValue)
        {
            Type = type;
            RequiredValue = requiredValue;
        }

        public TerminalEntry(TokenType type) : this(type, null) { }



    }
}
