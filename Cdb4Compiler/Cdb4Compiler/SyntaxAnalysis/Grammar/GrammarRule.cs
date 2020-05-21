using Cdb4Compiler.SyntaxAnalysis.Grammar.GrammarEntries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.SyntaxAnalysis.Grammar
{
    public class GrammarRule
    {
        public NonTermEntry Result { get; private set; }
        public GrammarEntrySet[] Patterns { get; private set; }

        public GrammarRule(NonTermEntry result, params GrammarEntrySet[] patterns)
        {
            Result = result;
            Patterns = patterns;
        }



    }
}
