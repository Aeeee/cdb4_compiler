using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.SyntaxAnalysis.Grammar.GrammarEntries
{
    public class NonTermEntry : GrammarEntry
    {
        public NonTermType Type { get; private set; }

        public NonTermEntry(NonTermType type)
        {
            Type = type;
        }

    }
}
