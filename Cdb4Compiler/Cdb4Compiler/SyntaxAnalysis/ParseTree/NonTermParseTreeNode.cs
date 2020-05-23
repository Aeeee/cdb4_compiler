using Cdb4Compiler.SyntaxAnalysis.Grammar.GrammarEntries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.SyntaxAnalysis.ParseTree
{
    public class NonTermParseTreeNode : ParseTreeNode
    {
        public NonTermType Type { get; private set; }

        public NonTermParseTreeNode(NonTermType type)
        {
            Type = type;
        }

        public override string ToString()
        {
            return $"[NonTerm({Type})]";
        }

    }
}
