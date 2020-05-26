using Cdb4Compiler.DataFormatting;
using Cdb4Compiler.LexicalAnalysis.Tokens;
using Cdb4Compiler.SyntaxAnalysis.Ast;
using Cdb4Compiler.SyntaxAnalysis.Grammar.GrammarEntries;
using Cdb4Compiler.SyntaxAnalysis.ParseTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.SyntaxAnalysis.AstBuilderImpl
{
    public class ParseTreeAstBuilder
    {
        public ProgramNode BuildAST(ParseTreeNode parseTreeRoot)
        {
            parseTreeRoot = SimplifyTree(parseTreeRoot);
            ParseTreeOutput.Print(parseTreeRoot);

            ProgramNode root = new ProgramNode();
            BuildAST(root, parseTreeRoot);
            return root;
        }

        private void BuildAST(ProgramNode root, ParseTreeNode node)
        {
            if (node is TermParseTreeNode)
                return;

            var nt = node as NonTermParseTreeNode;
            if (nt.Type == NonTermType.PROGRAM)
            {
                foreach (var child in nt.Children)
                    BuildAST(root, child);
                return;
            }

            if (nt.Type == NonTermType.VAR_DECL)
            {
                var varNode = new VarDeclNode();
                foreach (var child in nt.Children)
                {
                    var childTerm = child as TermParseTreeNode;
                    varNode.AddChild(new IdentifierNode(childTerm.Token.Text));
                }
                root.AddOperation(varNode);
            }


        }

        private ParseTreeNode SimplifyTree(ParseTreeNode parseTreeRoot)
        {
            var root = CopyTree(parseTreeRoot);
            SimplifyTree(root, null);
            return root;
        }

        private void SimplifyTree(ParseTreeNode node, NonTermParseTreeNode parent)
        {
            if (node is TermParseTreeNode)
            {
                var term = node as TermParseTreeNode;
                switch (term.Token.Type)
                {
                    case TokenType.END_TEXT:
                    case TokenType.KEYWORD:
                    case TokenType.PARENTHESES:
                    case TokenType.SPECIAL_SYMBOL:
                        parent.RemoveChild(node);
                        break;
                }
                return;
            }
            var nt = node as NonTermParseTreeNode;
            switch (nt.Type)
            {
                case NonTermType.ASSIGNMENT:
                case NonTermType.PROGRAM:
                case NonTermType.VAR_DECL:
                case NonTermType.CONDITION:
                case NonTermType.EXPRESSION:
                case NonTermType.OPERATOR:
                    {
                        var children = new List<ParseTreeNode>(nt.Children);
                        foreach (var child in children)
                            SimplifyTree(child, nt);
                        return;
                    }
                default:
                    break;
            }

            parent.RemoveChild(node);
            var nodeChildren = new List<ParseTreeNode>(nt.Children);
            foreach (var child in nodeChildren)
            {
                parent.AddChild(child);
                SimplifyTree(child, parent);
            }
        }

        private ParseTreeNode CopyTree(ParseTreeNode root)
        {
            if (root is TermParseTreeNode)
                return new TermParseTreeNode(((TermParseTreeNode)root).Token);
            var nt = root as NonTermParseTreeNode;
            var node = new NonTermParseTreeNode(nt.Type);
            foreach (var child in nt.Children)
                node.AddChild(CopyTree(child));
            return node;
        }

    }
}
