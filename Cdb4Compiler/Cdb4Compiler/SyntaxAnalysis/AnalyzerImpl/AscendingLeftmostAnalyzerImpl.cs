using Cdb4Compiler.LexicalAnalysis.Tokens;
using Cdb4Compiler.SyntaxAnalysis.Grammar;
using Cdb4Compiler.SyntaxAnalysis.Grammar.GrammarEntries;
using Cdb4Compiler.SyntaxAnalysis.ParseTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.SyntaxAnalysis.AnalyzerImpl
{
    public class AscendingLeftmostAnalyzerImpl
    {
        public void Analyze(IReadOnlyList<Token> tokens)
        {
            if (tokens.Count == 0)
                return;
            var top = BuildInitialTop(tokens);

            int i = 1;
            while (top.Count > 1 || !IsNonTermOfType(top[0], NonTermType.PROGRAM))
            {
                PrintTop(top);

                if (Reduce(top))
                    continue;

                if (CanShift(i, tokens))
                {
                    Shift(top, tokens, i);
                    i += 1;
                    continue;
                }

                Console.WriteLine("Error: cannot shift or reduce anymore.");
                break;
            }

        }

        private void PrintTop(List<ParseTreeNode> top)
        {
            Console.WriteLine($"Current top state ({top.Count} nodes):");
            foreach (var node in top)
                Console.WriteLine(node);
            Console.WriteLine("---\n");
        }

        private bool Reduce(List<ParseTreeNode> top)
        {
            for (int i = top.Count - 1; i >= 0; i--)
            {
                int length = top.Count - i;
                var nonterm = GrammarRules.MatchReduce(top, i, length);
                if (nonterm != null)
                {
                    top.RemoveRange(i, length);
                    top.Insert(i, new NonTermParseTreeNode(nonterm.Type));
                    Console.WriteLine("Reduce");
                    return true;
                }
            }

            return false;
        }

        private bool CanShift(int i, IReadOnlyList<Token> tokens)
        {
            return i < tokens.Count;
        }

        private void Shift(List<ParseTreeNode> top, IReadOnlyList<Token> tokens, int i)
        {
            top.Add(new TermParseTreeNode(tokens[i]));
            Console.WriteLine("Shift");
        }

        private List<ParseTreeNode> BuildInitialTop(IReadOnlyList<Token> tokens)
        {
            var top = new List<ParseTreeNode>();
            top.Add(new TermParseTreeNode(tokens[0]));
            return top;
        }

        private bool IsNonTermOfType(ParseTreeNode node, NonTermType type)
        {
            if (!(node is NonTermParseTreeNode))
                return false;
            var nt = node as NonTermParseTreeNode;
            return nt.Type == type;
        }

    }
}
