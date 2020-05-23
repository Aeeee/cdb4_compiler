using Cdb4Compiler.SyntaxAnalysis.Grammar.GrammarEntries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.SyntaxAnalysis.Grammar
{
    public class GrammarRestrictions
    {
        public GrammarEntry[] FollowEntries { get; private set; }

        public GrammarRestrictions(params GrammarEntry[] followEntries)
        {
            FollowEntries = followEntries;
        }

    }
}
