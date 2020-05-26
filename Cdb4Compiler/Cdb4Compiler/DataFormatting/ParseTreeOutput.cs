using Cdb4Compiler.SyntaxAnalysis.ParseTree;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Cdb4Compiler.DataFormatting
{
    public class ParseTreeOutput
    {
        public static void Print(ParseTreeNode root)
        {
            Print(root, 0, new HashSet<int>());
        }

        private static void Print(ParseTreeNode node, int indent, HashSet<int> ignoreIndents)
        {
            //for (int j = 0; j < 1; j++)
            //{
            //    for (int i = 0; i < indent; i++)
            //        Console.Write("|   ");
            //    Console.Write('\n');
            //}

            for (int i = 0; i < indent; i++)
            {
                if (i == indent - 1)
                    Console.Write("|-----");
                else if (!ignoreIndents.Contains(i))
                    Console.Write("|     ");
                else
                    Console.Write("      ");
            }

            if (node is TermParseTreeNode)
            {
                var term = node as TermParseTreeNode;
                Console.Write($"Terminal[{term.Token.Type}, '{term.Token.Text}']\n");
            }
            else
            {
                var nonterm = node as NonTermParseTreeNode;
                Console.Write($"NonTerminal[{nonterm.Type}]\n");

                foreach (var child in nonterm.Children)
                {
                    HashSet<int> newIgnoreIndents = new HashSet<int>(ignoreIndents);
                    if (child == nonterm.Children.Last())
                        newIgnoreIndents.Add(indent);
                    Print(child, indent + 1, newIgnoreIndents);
                }
            }
        }

    }
}
