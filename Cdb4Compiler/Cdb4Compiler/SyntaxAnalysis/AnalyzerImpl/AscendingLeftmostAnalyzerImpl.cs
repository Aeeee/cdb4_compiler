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
        public void Analyze(IReadOnlyList<Token> tokens, List<SyntaxError> errors, ref ParseTreeNode root)
        {
            if (tokens.Count == 0)
                return;
            var top = BuildInitialTop(tokens);
            TermParseTreeNode next = PeekNextTerm(tokens, 1);

            int i = 1;
            while (top.Count > 1 || !IsNonTermOfType(top[0], NonTermType.PROGRAM))
            {
                //PrintTop(top);

                if (Reduce(top, next))
                    continue;

                if (next != null)
                {
                    Shift(top, next, ref i);
                    next = PeekNextTerm(tokens, i);
                    continue;
                }
                break;
            }

            if (top.Count != 1 || !IsNt(top[0], NonTermType.PROGRAM))
            {
                DetectErrors(new List<ParseTreeNode>(top), errors);
                root = null;
            }
            else
                root = top[0];

            //PrintTop(top);
        }

        private void DetectErrors(List<ParseTreeNode> top, List<SyntaxError> errors)
        {
            
            //Console.WriteLine("Error: cannot shift or reduce anymore.");

            if (top[0] is NonTermParseTreeNode && ((NonTermParseTreeNode)top[0]).Type == NonTermType.VAR_LIST)
                errors.Add(new SyntaxError(1, "Variable declaration block is incomplete (missing a semicolon?)."));
            else if (!(top[0] is NonTermParseTreeNode) || ((NonTermParseTreeNode)top[0]).Type != NonTermType.VAR_DECL)
                errors.Add(new SyntaxError(1, "Missing variable declaration block."));

            for (int i = 0; i < top.Count; i++)
            {
                ParseTreeNode cur = top[i];
                ParseTreeNode prev = i > 0 ? top[i - 1] : null;
                ParseTreeNode prev2 = i > 1 ? top[i - 2] : null;

                if (IsNt(cur, NonTermType.PROGRAM) && i > 0)
                {
                    errors.Add(new SyntaxError(1, "Found code before variable declaration block."));
                    continue;
                }

                if (IsT(cur, TokenType.PARENTHESES))
                {
                    errors.Add(new SyntaxError(LineFromT(cur), "One or more unmatched parentheses."));
                    continue;
                }

                if (IsT(cur, TokenType.ASSIGNMENT_OP) && !IsT(prev, TokenType.IDENTIFIER))
                {
                    errors.Add(new SyntaxError(LineFromT(cur), "Left side of an assignment must be an identifier."));
                    continue;
                }

                if (IsT(prev, TokenType.ASSIGNMENT_OP) && !IsNt(cur, NonTermType.EXPRESSION))
                {
                    errors.Add(new SyntaxError(LineFromT(prev), "Right side of an assignment should be an expression."));
                    continue;
                }

                if (IsT(prev2, TokenType.ASSIGNMENT_OP) && !IsT(cur, TokenType.SPECIAL_SYMBOL, ";"))
                {
                    errors.Add(new SyntaxError(LineFromT(prev2), "Might be missing semicolon after assignment."));
                    continue;
                }

                if (IsT(prev, TokenType.KEYWORD, "IF") && !IsNt(cur, NonTermType.EXPRESSION))
                {
                    errors.Add(new SyntaxError(LineFromT(prev), "Expression expected after keyword IF."));
                    continue;
                }

                if (IsT(prev2, TokenType.KEYWORD, "IF") && !IsT(cur, TokenType.KEYWORD, "THEN"))
                {
                    errors.Add(new SyntaxError(LineFromT(prev2), "Keyword THEN expected after conditional expression."));
                    continue;
                }

                if (IsT(prev, TokenType.KEYWORD, "THEN") && !IsNt(cur, NonTermType.OPERATOR))
                {
                    errors.Add(new SyntaxError(LineFromT(prev), "Operator expected after THEN keyword."));
                    continue;
                }

                if (IsT(cur, TokenType.KEYWORD, "ELSE") && !IsT(prev2, TokenType.KEYWORD, "THEN"))
                {
                    errors.Add(new SyntaxError(LineFromT(cur), "Keyword ELSE without IF/THEN conditional operators before it."));
                    continue;
                }

                if (IsT(prev, TokenType.KEYWORD, "ELSE") && !IsNt(cur, NonTermType.OPERATOR))
                {
                    errors.Add(new SyntaxError(LineFromT(prev), "Operator expected after ELSE keyword."));
                    continue;
                }

            }

            if (errors.Count == 0)
                errors.Add(new SyntaxError(-1, "Wrong program structure."));
        }

        private bool IsNt(ParseTreeNode node, NonTermType type)
        {
            if (!(node is NonTermParseTreeNode))
                return false;
            var nt = node as NonTermParseTreeNode;
            return nt.Type == type;
        }

        private bool IsT(ParseTreeNode node, TokenType type)
        {
            if (!(node is TermParseTreeNode))
                return false;
            var t = node as TermParseTreeNode;
            return t.Token.Type == type;
        }

        private bool IsT(ParseTreeNode node, TokenType type, string value)
        {
            if (!(node is TermParseTreeNode))
                return false;
            var t = node as TermParseTreeNode;
            return t.Token.Type == type && t.Token.Text == value;
        }

        private int LineFromT(ParseTreeNode node)
        {
            if (!(node is TermParseTreeNode))
                return -1;
            return ((TermParseTreeNode)node).Token.AtLine;
        }

        private void Unshift(List<ParseTreeNode> top, int i)
        {
            if (!(top[i] is NonTermParseTreeNode))
                return;
            var node = top[i] as NonTermParseTreeNode;
            var children = node.Children;

            top.RemoveAt(i);

            for (int j = 0; j < children.Count; j++)
                top.Insert(i + j, children[j]);
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
            for (int i = 0; i < top.Count; i++)
            {
                int length = top.Count - i;
                var nonterm = GrammarRules.MatchReduce(top, i, length, next);
                if (nonterm != null)
                {
                    var node = new NonTermParseTreeNode(nonterm.Type);
                    for (int j = i; j < i + length; j++)
                        node.AddChild(top[j]);

                    top.RemoveRange(i, length);
                    top.Insert(i, node);
                    //Console.WriteLine("Reduce");
                    return true;
                }
            }

            return false;
        }

        private void Shift(List<ParseTreeNode> top, TermParseTreeNode next, ref int i)
        {
            top.Add(next);
            i += 1;
            //Console.WriteLine("Shift");
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
