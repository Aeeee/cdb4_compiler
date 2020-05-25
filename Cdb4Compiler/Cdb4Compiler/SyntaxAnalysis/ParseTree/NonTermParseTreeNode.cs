using Cdb4Compiler.SyntaxAnalysis.Grammar.GrammarEntries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.SyntaxAnalysis.ParseTree
{
    public class NonTermParseTreeNode : ParseTreeNode
    {
        public NonTermType Type { get; private set; }
        public IReadOnlyList<ParseTreeNode> Children { get => children; }
        private List<ParseTreeNode> children = new List<ParseTreeNode>();
        public override int ChildCount => children.Count;

        public NonTermParseTreeNode(NonTermType type)
        {
            Type = type;
        }

        public void AddChild(ParseTreeNode node)
        {
            children.Add(node);
        }

        public override string ToString()
        {
            return $"[NonTerm({Type})]";
        }

    }
}
