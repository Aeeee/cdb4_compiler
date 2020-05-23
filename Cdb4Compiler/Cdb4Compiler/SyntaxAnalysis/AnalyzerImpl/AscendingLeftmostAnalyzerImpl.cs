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
            TermParseTreeNode next = PeekNextTerm(tokens, 1);

            int i = 1;
            while (top.Count > 1 || !IsNonTermOfType(top[0], NonTermType.PROGRAM))
            {
                PrintTop(top);

                if (Reduce(top, next))
                    continue;

                if (next != null)
                {
                    Shift(top, next, ref i);
                    next = PeekNextTerm(tokens, i);
                    continue;
                }

                Console.WriteLine("Error: cannot shift or reduce anymore.");
                break;
            }

            PrintTop(top);
        }

        private TermParseTreeNode PeekNextTerm(IReadOnlyList<Token> tokens, int i)
        {
            return tokens.Count > i ? new TermParseTreeNode(tokens[i]) : null;
        }

        private void PrintTop(List<ParseTreeNode> top)
        {
            Console.WriteLine($"Current top state ({top.Count} nodes):");
            foreach (var node in top)
                Console.WriteLine(node);
            Console.WriteLine("---\n");
        }

        private bool Reduce(List<ParseTreeNode> top, ParseTreeNode next)
        {
            //for (int i = top.Count - 1; i >= 0; i--)
            for (int i = 0; i < top.Count; i++)
            {
                int length = top.Count - i;
                var nonterm = GrammarRules.MatchReduce(top, i, length, next);
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

        private void Shift(List<ParseTreeNode> top, TermParseTreeNode next, ref int i)
        {
            top.Add(next);
            i += 1;
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
