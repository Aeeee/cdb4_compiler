using Cdb4Compiler.SyntaxAnalysis.ParseTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.SyntaxAnalysis.Grammar.GrammarEntries
{
    public class GrammarEntrySet
    {
        public GrammarRestrictions Restrictions { get; private set; }

        private List<GrammarEntry> entries;
        public IReadOnlyList<GrammarEntry> Entries { get => entries; }
        public int Length { get => entries.Count; }

        public GrammarEntrySet(GrammarRestrictions restrictions, params GrammarEntry[] entryArr)
        {
            Restrictions = restrictions;
            entries = new List<GrammarEntry>(entryArr);
        }

    }
}
