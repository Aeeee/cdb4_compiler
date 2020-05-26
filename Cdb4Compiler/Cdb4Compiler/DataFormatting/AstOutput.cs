using Cdb4Compiler.SyntaxAnalysis.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdb4Compiler.DataFormatting
{
    public class AstOutput
    {
        public static void Print(AstNode root)
        {
            Print(root, 0, new HashSet<int>());
        }

        private static void Print(AstNode node, int indent, HashSet<int> ignoreIndents)
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

            Console.Write(node.ToString() + "\n");

            if (node is ProgramNode)
            {
                var children = ((ProgramNode)node).Operations;
                foreach (var child in children)
                {
                    HashSet<int> newIgnoreIndents = new HashSet<int>(ignoreIndents);
                    if (child == children.Last())
                        newIgnoreIndents.Add(indent);
                    Print(child, indent + 1, newIgnoreIndents);
                }
            }

            if (node is VarDeclNode)
            {
                var children = ((VarDeclNode)node).Children;
                foreach (var child in children)
                {
                    HashSet<int> newIgnoreIndents = new HashSet<int>(ignoreIndents);
                    if (child == children.Last())
                        newIgnoreIndents.Add(indent);
                    Print(child, indent + 1, newIgnoreIndents);
                }
            }

            if (node is IHasLeftAstNode)
            {
                HashSet<int> newIgnoreIndents = new HashSet<int>(ignoreIndents);
                if (!(node is IHasRightAstNode))
                    newIgnoreIndents.Add(indent);
                Print(((IHasLeftAstNode)node).Left, indent + 1, newIgnoreIndents);
            }

            if (node is IHasRightAstNode)
            {
                HashSet<int> newIgnoreIndents = new HashSet<int>(ignoreIndents);
                newIgnoreIndents.Add(indent);
                Print(((IHasRightAstNode)node).Right, indent + 1, newIgnoreIndents);
            }

            if (node is ConditionNode)
            {
                HashSet<int> newIgnoreIndents = new HashSet<int>(ignoreIndents);
                newIgnoreIndents.Add(indent);

                var cond = node as ConditionNode;
                Print(cond.Condition, indent + 1, ignoreIndents);
                
                if (cond.Else != null)
                {
                    Print(cond.Then, indent + 1, ignoreIndents);
                    Print(cond.Else, indent + 1, newIgnoreIndents);
                }
                else
                    Print(cond.Then, indent + 1, newIgnoreIndents);
            }

        }


    }
}
