using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.SyntaxAnalysis.Grammar.GrammarEntries
{
    public class GrammarEntrySet
    {
        private List<GrammarEntry> entries;
        public IReadOnlyList<GrammarEntry> Entries { get => entries; }

        public GrammarEntrySet(params GrammarEntry[] entryArr)
        {
            entries = new List<GrammarEntry>(entryArr);
        }



    }
}
