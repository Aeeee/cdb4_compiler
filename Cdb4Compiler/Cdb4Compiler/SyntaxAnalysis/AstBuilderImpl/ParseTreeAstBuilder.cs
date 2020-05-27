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
            //ParseTreeOutput.Print(parseTreeRoot);

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

            if (nt.Type == NonTermType.ASSIGNMENT)
            {
                var binOpNode = ParseAssignment(node);
                root.AddOperation(binOpNode);
            }

            if (nt.Type == NonTermType.CONDITION)
            {
                var condNode = ParseCondition(node);
                root.AddOperation(condNode);
            }
        }

        private AstNode ParseAssignment(ParseTreeNode node)
        {
            var nt = node as NonTermParseTreeNode;
            var left = ParseNode(nt.Children[0]);
            var right = ParseNode(nt.Children[2]);
            return new BinaryOpNode(":=", left, right);
        }

        private AstNode ParseCondition(ParseTreeNode node)
        {
            var nt = node as NonTermParseTreeNode;
            var cond = ParseNode(nt.Children[0]);
            var thenNode = ParseNode(nt.Children[1]);
            AstNode elseNode = nt.ChildCount > 2 ? ParseNode(nt.Children[2]) : null;
            return new ConditionNode(cond, thenNode, elseNode);
        }

        private AstNode ParseNode(ParseTreeNode node)
        {
            if (node is TermParseTreeNode)
            {
                var term = node as TermParseTreeNode;
                if (term.Token.Type == TokenType.IDENTIFIER)
                    return new IdentifierNode(term.Token.Text);
                if (term.Token.Type == TokenType.CONSTANT)
                    return new ConstantNode(term.Token.Text);

                throw new Exception("Unsupported terminal parse node type: " + term.Token.Type);
            }

            var nt = node as NonTermParseTreeNode;
            if (nt.Type == NonTermType.ASSIGNMENT)
                return ParseAssignment(node);
            if (nt.Type == NonTermType.CONDITION)
                return ParseCondition(node);

            if (nt.Type != NonTermType.EXPRESSION)
                throw new Exception("Unsupported non-terminal parse node type: " + nt.Type);

            if (nt.ChildCount == 1)
                return ParseNode(nt.Children[0]);
            if (nt.ChildCount == 2)
            {
                var unaryOp = nt.Children[0] as TermParseTreeNode;
                var right = ParseNode(nt.Children[1]);

                if (unaryOp.Token.Type != TokenType.MATH_OP || unaryOp.Token.Text != "-")
                    throw new Exception("Unsupported unary operator: " + unaryOp.Token.Text);
                return new UnaryOpNode(unaryOp.Token.Text, right);
            }
            if (nt.ChildCount == 3)
            {
                var left = ParseNode(nt.Children[0]);
                var right = ParseNode(nt.Children[2]);
                var op = nt.Children[1] as TermParseTreeNode;

                return new BinaryOpNode(op.Token.Text, left, right);
            }

            throw new Exception("More than 3 children in expression are not supported.");
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
